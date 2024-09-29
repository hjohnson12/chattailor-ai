using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Events.EventArgs;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI.Requests;
using ChatTailorAI.Shared.Models.Chat.OpenAI.Responses;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Resources;
using ChatTailorAI.Shared.Services.Authentication;
using ChatTailorAI.Shared.Services.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Services.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.Chat.OpenAI
{
    public class OpenAIChatService : IOpenAIChatService
    {
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private IEventAggregator _eventAggregator;
        private IAuthenticationService _authenticationService;
        private IToolExecutorService _toolExecutorService;
        private static HttpClient _httpClient;
        private CancellationTokenSource _cancellationTokenSource;
        private const string ChatCompletionsEndpoint = "v1/chat/completions";
        private readonly List<RootObject> _functions;

        public OpenAIChatService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            IEventAggregator eventAggregator,
            IAuthenticationService authenticationService,
            IToolExecutorService toolExecutorService,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;
            _eventAggregator = eventAggregator;
            _authenticationService = authenticationService;
            _toolExecutorService = toolExecutorService;

            FunctionsEnabled = _userSettingsService.Get<bool>(UserSettings.FunctionsEnabled); ;

            _eventAggregator.ApiKeyChanged += OnApiKeyChanged;

            var openAiApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            client = new HttpClient { Timeout = TimeSpan.FromMinutes(4) };
            client.BaseAddress = new Uri("https://api.openai.com/");
            client.Timeout = TimeSpan.FromMinutes(4);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
            _httpClient = client;

            _functions = RetrieveSelectedFunctions();

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public bool FunctionsEnabled { get; set; }

        private List<RootObject> RetrieveSelectedFunctions()
        {
            string[] selectedFunctions = _userSettingsService
                .Get<string>(UserSettings.FunctionsSelected)
                .Split(',');

            string jsonSchemas = File.ReadAllText("Data/Schemas/function-schemas.json");
            List<RootObject> functionSchemas = JsonConvert.DeserializeObject<List<RootObject>>(jsonSchemas);
            return functionSchemas
                .Where(f => selectedFunctions.Contains(f.Name))
                .ToList();
        }

        private void OnApiKeyChanged(object sender, ApiKeyChangedEventArgs e)
        {
            if (e.KeyType == ApiKeyType.OpenAI)
                UpdateAuthorizationHeader(e.ApiKey);
        }

        public async Task<OpenAIChatResponseDto> GenerateChatResponseAsync(OpenAIChatRequest chatRequest)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            if (chatRequest.Instructions != null && chatRequest.Instructions != "")
            {
                chatRequest.Messages.Insert(0, new OpenAIBaseChatMessageDto
                {
                    Role = "system",
                    Content = chatRequest.Instructions
                });
            }

            var body = BuildChatCompletionRequest(chatRequest);

            FunctionsEnabled = _userSettingsService.Get<bool>(UserSettings.FunctionsEnabled);
            if (FunctionsEnabled == true && !ModelConstants.ModelsWithoutFunctionSupport.Contains(body.Model))
            {
                body.FunctionCall = "auto";
                body.Functions = this._functions;
            }

            using (var response = await SendChatCompletionRequest(body, ChatCompletionsEndpoint, _cancellationTokenSource))
            {
                if (IsStreamingEnabled(chatRequest))
                {
                    return await ProcessStreamResponse(chatRequest, response);
                }

                return await ProcessNonStreamResponse(chatRequest, response);
            }
        }

        private OpenAIChatCompletionRequest BuildChatCompletionRequest(OpenAIChatRequest chatRequest)
        {
            bool isStreamingEnabled = IsStreamingEnabled(chatRequest);
            if (IsBetaReasoningModel(chatRequest.Model))
            {
                return BuildReasoningModelRequest(chatRequest);
            }

            return BuildStandardRequest(chatRequest, isStreamingEnabled);
        }

        private OpenAIChatCompletionRequest BuildStandardRequest(OpenAIChatRequest chatRequest, bool shouldStream)
        {
            return new OpenAIChatCompletionRequest
            {
                Model = chatRequest.Model,
                Messages = chatRequest.Messages,
                MaxCompletionTokens = chatRequest.Settings.MaxTokens,
                N = 1,
                Stop = null,
                FrequencyPenalty = chatRequest.Settings.FrequencyPenalty,
                PresencePenalty = chatRequest.Settings.PresencePenalty,
                Temperature = chatRequest.Settings.Temperature,
                Stream = shouldStream
            };
        }

        private OpenAIChatCompletionRequest BuildReasoningModelRequest(OpenAIChatRequest chatRequest)
        {
            // See beta reasoning model limitations
            // https://platform.openai.com/docs/guides/reasoning/beta-limitations

            var messages = chatRequest.Messages;
            if (chatRequest.Messages.Any(m => m.Role.Equals("system")))
            {
                messages = messages.Where(m => !m.Role.Equals("system", StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return new OpenAIChatCompletionRequest
            {
                Model = chatRequest.Model,
                Messages = messages,
                N = 1,
                MaxCompletionTokens = chatRequest.Settings.MaxTokens,
            };
        }

        private bool IsStreamingEnabled(OpenAIChatRequest chatRequest)
        {
            return !ModelConstants.ModelsWithoutStreamingSupport.Contains(chatRequest.Model) 
                && chatRequest.Settings.Stream
                && !IsBetaReasoningModel(chatRequest.Model);
        }

        private bool IsBetaReasoningModel(string model)
        {
            return model.StartsWith("o1-preview") || model.StartsWith("o1-mini");
        }

        private async Task<OpenAIChatResponseDto> ProcessNonStreamResponse(OpenAIChatRequest chatRequest, HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<OpenAIChatCompletionErrorResponse>(errorContent);

                throw new HttpRequestException($"OpenAI Request failed with status code {response.StatusCode}: {errorResponse.Error.Message}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var chatCompletion = JsonConvert.DeserializeObject<OpenAIChatCompletionResponse>(responseContent);
            var message = chatCompletion.Choices[0].Message;
            return new OpenAIChatResponseDto() { Content = message.Content , Role = "assistant" };
        }

        private async Task<OpenAIChatResponseDto> ProcessStreamResponse(OpenAIChatRequest chatRequest, HttpResponseMessage response)
        {
            var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
            var assistantMessage = new StringBuilder();
            var functionArguments = new StringBuilder();
            var functionName = string.Empty;
            var assistantRole = "assistant";

            while (!streamReader.EndOfStream)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }

                var line = await streamReader.ReadLineAsync();
                if (line.Equals("data: [DONE]"))
                {
                    break;
                }

                if (line.StartsWith("data: "))
                {
                    var data = line.Substring(6);
                    OpenAIChatCompletionResponse chatCompletion = JsonConvert.DeserializeObject<OpenAIChatCompletionResponse>(data);

                    if (chatCompletion == null || chatCompletion.Choices == null)
                    {
                        var error = JsonConvert.DeserializeObject<OpenAIChatCompletionErrorResponse>(data);
                        throw new Exception($"{error.Error.Message}");
                    }

                    // Process delta based on the type
                    if (chatCompletion.Choices[0].Delta.Role != null)
                    {
                        assistantRole = chatCompletion.Choices[0].Delta.Role;

                        if (chatCompletion.Choices[0].Delta.FunctionCall != null)
                        {
                            functionName = chatCompletion.Choices[0].Delta.FunctionCall.Name;
                            functionArguments.Append(chatCompletion.Choices[0].Delta.FunctionCall.Arguments);
                        }
                    }
                    else if (chatCompletion.Choices[0].Delta.Role == null &&
                        chatCompletion.Choices[0].Delta.FunctionCall != null)
                    {
                        functionArguments.Append(chatCompletion.Choices[0].Delta.FunctionCall.Arguments);
                    }
                    else if (chatCompletion.Choices[0].Delta.Content != null)
                    {
                        assistantMessage.Append(chatCompletion.Choices[0].Delta.Content);
                        // TODO: Maybe update message in messages here too so it will at least
                        // contain some of the message if an error occurs to avoid lost tokens

                        if (chatRequest.Settings.Stream)
                        {
                            var responseDto = new OpenAIChatResponseDto
                            {
                                Content = assistantMessage.ToString(),
                                Role = assistantRole
                            };

                            _eventAggregator.PublishMessageReceived(new MessageReceivedEvent
                            {
                                Message = responseDto
                            });
                        }
                    }
                    else if (chatCompletion.Choices[0].Delta.Role == null && chatCompletion.Choices[0].Delta.Content == null)
                    {
                        // Stream is finished
                    }
                }
            }

            if (functionArguments.ToString() != "" && functionName != string.Empty)
            {
                var functionCall = new FunctionCall
                {
                    Arguments = functionArguments.ToString(),
                    Name = functionName
                };

                string functionResult = await HandleFunctionCall(functionCall);
                OpenAIChatResponseDto functionCallResponseDto =
                    await HandleFunctionCallResult(functionCall, functionResult, chatRequest);

                return functionCallResponseDto;
            }

            return new OpenAIChatResponseDto { Content = assistantMessage.ToString(), Role = "assistant" };
        }

        private async Task<string> HandleFunctionCall(FunctionCall functionCall)
        {
            string functionName = functionCall.Name;
            JObject functionArgs = JObject.Parse(functionCall.Arguments);

            if (IsAuthenticationRequiredForFunction(functionName))
            {
                if (!await AuthenticateSpotifyUser())
                {
                    return "Failure to authenticate with Spotify. Please try again.";
                }
            }

            var toolArguments = functionArgs.ToObject<Dictionary<string, string>>();
            string functionResult = await _toolExecutorService.Execute(functionName, toolArguments);

            return functionResult;
        }

        private async Task<OpenAIChatResponseDto> HandleFunctionCallResult(FunctionCall functionCall, string functionResult, OpenAIChatRequest chatRequest)
        {
            if (functionResult == null || functionResult == string.Empty)
            {
                return new OpenAIChatResponseDto
                {
                    Content = "Unknown chained function call detected, the model sometimes tries to call one twice. Please try again.",
                    Role = "assistant"
                };
            }

            var modelResponseMsg = new OpenAIBaseChatMessageDto
            {
                Role = "assistant",
                FunctionCall = functionCall,
                Content = null
            };
            chatRequest.Messages.Add(modelResponseMsg);

            var functionResultMessage = new OpenAIBaseChatMessageDto
            {
                Role = "function",
                Name = functionCall.Name,
                Content = functionResult
            };
            chatRequest.Messages.Add(functionResultMessage);

            return await GenerateChatResponseAsync(chatRequest);
        }

        public async Task<HttpResponseMessage> SendChatCompletionRequest(OpenAIChatCompletionRequest body, string endpoint, CancellationTokenSource cancellationTokenSource)
        {
            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                _cancellationTokenSource.Token);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    throw new Exception($"Service unavailable");
                }

                var data = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"API Error: {data}");
                var errorResponse = JsonConvert.DeserializeObject<OpenAIChatCompletionErrorResponse>(data);

                throw new Exception($"{errorResponse.Error.Message}");
            }
            else
            {
                return response;
            }
        }

        public void UpdateAuthorizationHeader(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public void CancelStream()
        {
            _cancellationTokenSource.Cancel();
        }

        private bool IsAuthenticationRequiredForFunction(string functionName)
        {
            var functionsRequiringAuth = new HashSet<string>
            {
                "get_my_playlist_names",
                "get_playlist_songs",
                "add_song_to_playlist",
                "search_and_play_song"
            };

            return functionsRequiringAuth.Contains(functionName);
        }

        private async Task<bool> AuthenticateSpotifyUser()
        {
            bool validToken = _authenticationService.ValidateSpotifyAccessToken();
            if (!validToken)
            {
                return await _authenticationService.BeginSpotifyAuthentication();
            }
            return true;
        }

        public bool ValidateApiKey()
        {
            var openAiApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            return !string.IsNullOrEmpty(openAiApiKey);
        }
    }
}