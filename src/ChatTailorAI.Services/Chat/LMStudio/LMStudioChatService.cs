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
using Newtonsoft.Json;
using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Chat.LMStudio;
using ChatTailorAI.Shared.Models.Chat.LMStudio.Requests;
using ChatTailorAI.Shared.Models.Chat.LMStudio.Responses;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Authentication;
using ChatTailorAI.Shared.Services.Chat.LMStudio;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Services.Tools;

namespace ChatTailorAI.Services.Chat.LMStudio
{
    public class LMStudioChatService : ILMStudioChatService
    {
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private IEventAggregator _eventAggregator;
        private IAuthenticationService _authenticationService;
        private IToolExecutorService _toolExecutorService;
        private static HttpClient _httpClient;
        private CancellationTokenSource _cancellationTokenSource;
        private const string ChatCompletionsEndpoint = "v1/chat/completions";

        public LMStudioChatService(
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

            var lmStudioServerUrl = _appSettingsService.LMStudioServerUrl != null && _appSettingsService.LMStudioServerUrl != "" ?
                _appSettingsService.LMStudioServerUrl :
                _userSettingsService.Get<string>(UserSettings.LMStudioServerUrl);

            client = new HttpClient { Timeout = TimeSpan.FromMinutes(4) };
            if (lmStudioServerUrl != null && lmStudioServerUrl != "")
            {
                client.BaseAddress = new Uri(lmStudioServerUrl);
            }
            client.Timeout = TimeSpan.FromMinutes(4);
            client.DefaultRequestHeaders.Clear();
            _httpClient = client;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<LMStudioChatResponseDto> GenerateChatResponseAsync(LMStudioChatRequest chatRequest)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            if (chatRequest.Instructions != null && chatRequest.Instructions != "")
            {
                chatRequest.Messages.Insert(0, new LMStudioBaseChatMessageDto
                {
                    Role = "system",
                    Content = chatRequest.Instructions
                });
            }

            chatRequest.Model = chatRequest.Model.Replace("@lmstudio/", "");

            var body = new LMStudioChatCompletionRequest
            {
                Model = chatRequest.Model,
                Messages = chatRequest.Messages,
                MaxTokens = chatRequest.Settings.MaxTokens,
                Temperature = chatRequest.Settings.Temperature,
                Stream = true // Always stream ?
            };


            using (var response = await SendChatCompletionRequest(body, ChatCompletionsEndpoint, _cancellationTokenSource))
            {
                //response.EnsureSuccessStatusCode();
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
                        LMStudioChatCompletionResponse chatCompletion = JsonConvert.DeserializeObject<LMStudioChatCompletionResponse>(data);

                        if (chatCompletion == null || chatCompletion.Choices == null)
                        {
                            var parsedJson = JsonConvert.DeserializeObject<Root>(data);
                            throw new Exception($"{parsedJson.Error.Message}");
                        }

                        // Process delta based on the type
                        if (chatCompletion.Choices[0].Delta.Content != null)
                        {
                            assistantRole = chatCompletion.Choices[0].Delta.Role;
                            assistantMessage.Append(chatCompletion.Choices[0].Delta.Content);
                            // TODO: Maybe update message in messages here too so it will at least
                            // contain some of the message if an error occurs

                            if (chatRequest.Settings.Stream)
                            {
                                var responseDto = new LMStudioChatResponseDto
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
                            //messages.Add(new ChatCompletionMessage { Role = assistantRole, Content = assistantMessage.ToString() });
                        }
                    }
                }

                return new LMStudioChatResponseDto { Content = assistantMessage.ToString(), Role = "assistant" };
            }
        }

        public async Task<HttpResponseMessage> SendChatCompletionRequest(LMStudioChatCompletionRequest body, string endpoint, CancellationTokenSource cancellationTokenSource)
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
                var errorResponse = JsonConvert.DeserializeObject<LMStudioChatErrorResponse>(data);

                throw new Exception($"{errorResponse.Error.Message}");
            }
            else
            {
                return response;
            }
        }

        public async Task<List<string>> GetModels()
        {
            try
            {
                // Make API call to /v1/models to get models
                var response = await _httpClient.GetAsync("v1/models");
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();
                var models = JsonConvert.DeserializeObject<LMStudioModelsResponse>(data);
                List<string> modelIds = models.GetModelIds();
                var processedModels = ProcessModelNames(modelIds);

                return processedModels;
            } 
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<string>();
            }
        }

        private List<string> ProcessModelNames(List<string> modelNames)
        {
            return modelNames.Select(modelName =>
            {
                var parts = modelName.Split('/');

                // Remove the last part (which contains the .gguf extension) and join the rest
                var processedName = string.Join("/", parts.Take(parts.Length - 1));

                return $"@lmstudio/{processedName}";
            }).ToList();
        }

        public void CancelStream()
        {
            _cancellationTokenSource.Cancel();
        }
    }

    public class ErrorModel
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public object Param { get; set; }
        public object Code { get; set; }
    }

    public class Root
    {
        public ErrorModel Error { get; set; }
    }
}