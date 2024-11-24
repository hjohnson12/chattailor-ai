using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.Google.Content;
using ChatTailorAI.Shared.Models.Chat.Google.Requests;
using ChatTailorAI.Shared.Models.Chat.Google.Responses;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Chat.Google;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.Chat.Google
{
    public enum FinishReason
    {
        FINISH_REASON_UNSPECIFIED,
        STOP,
        MAX_TOKENS,
        SAFETY,
        RECITATION,
        OTHER
    }

    public enum BlockReason
    {
        BLOCK_REASON_UNSPECIFIED,
        SAFETY,
        OTHER
    }

    public class ModelSetting
    {
        public int MaxOutputTokens { get; set; }
        public double Temperature { get; set; }
    }

    public class GoogleChatService : IGoogleChatService
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IEventAggregator _eventAggregator;

        private readonly HttpClient _httpClient;
        private CancellationTokenSource _cancellationTokenSource;


        private static readonly Dictionary<FinishReason, string> finishReasons = new Dictionary<FinishReason, string>
        {
            {FinishReason.FINISH_REASON_UNSPECIFIED, "Default value. This value is unused."},
            {FinishReason.STOP, "Natural stop point of the model or provided stop sequence."},
            {FinishReason.MAX_TOKENS, "The maximum number of tokens as specified in the request was reached."},
            {FinishReason.SAFETY, "The candidate content was flagged for safety reasons."},
            {FinishReason.RECITATION, "The candidate content was flagged for recitation reasons."},
            {FinishReason.OTHER, "Unknown reason."}
        };

        private static readonly Dictionary<BlockReason, string> blockReasons = new Dictionary<BlockReason, string>
        {
            {BlockReason.BLOCK_REASON_UNSPECIFIED, "Default value. This value is unused."},
            {BlockReason.SAFETY, "The candidate content was flagged for safety reasons."},
            {BlockReason.OTHER, "Prompt was blocked due to unknown reasons."}
        };

        private Dictionary<string, ModelSetting> modelSettings;

        public GoogleChatService(
            IUserSettingsService userSettingsService,
            IAppSettingsService appSettingsService,
            IEventAggregator eventAggregator,
            HttpClient client
            )
        {
            _userSettingsService = userSettingsService;
            _appSettingsService = appSettingsService;
            _eventAggregator = eventAggregator;

            _cancellationTokenSource = new CancellationTokenSource();

            //var googleApiKey = _appSettingsService.GoogleAIApiKey != null && _appSettingsService.GoogleAIApiKey != "" ?
            //    _appSettingsService.GoogleAIApiKey :
            //    _userSettingsService.Get<string>(UserSettings.GoogleAIApiKey);

            client = new HttpClient { Timeout = TimeSpan.FromMinutes(4) };
            client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
            client.Timeout = TimeSpan.FromMinutes(4);
            client.DefaultRequestHeaders.Clear();
            _httpClient = client;
        }

        public async Task<GoogleChatResponseDto> GenerateChatResponseAsync(GoogleChatRequest chatRequest)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // Remap messages to use Parts instead of Content for Google
            var body = CreateRequestBody(chatRequest);
            var (endpoint, modelMethod) = CreateEndpoint(chatRequest);

            using (var response = await SendMessagesRequest(body, endpoint, _cancellationTokenSource)) 
            {
                var assistantMessage = new StringBuilder();

                if (modelMethod.Equals("generateContent"))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (responseContent.Contains("{\n  \"error\""))
                    {
                        var errorResponse = JsonConvert.DeserializeObject<GoogleGenerateContentErrorResponse>(responseContent);
                        throw new Exception($"Failed to send request to Google Gemini API. Error: {errorResponse.Error.Message}");
                    }

                    var googleChatResponse = JsonConvert.DeserializeObject<GoogleGenerateContentResponse>(responseContent);
                    if (googleChatResponse.Candidates == null && googleChatResponse.PromptFeedback != null)
                    {
                        var contentFilterMessage = HandleContentFilteredMessage(googleChatResponse);
                        return new GoogleChatResponseDto { Content = contentFilterMessage, Role = "model" };
                    }

                    foreach (var candidate in googleChatResponse.Candidates)
                    {
                        foreach (var part in candidate.Content.Parts)
                        {
                            if (part is GoogleTextContentPart textPart) 
                            {
                                assistantMessage.Append(textPart.Text); 
                            }
                        }
                    }

                    return new GoogleChatResponseDto { Content = assistantMessage.ToString(), Role = "model" };
                }

                var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                while (!streamReader.EndOfStream)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    // TODO: Implement correct way with their API, need streaming json parser??

                    var data = await streamReader.ReadToEndAsync();

                    var googleChatResponses = JsonConvert.DeserializeObject<List<GoogleGenerateContentResponse>>(data);
                    if (googleChatResponses.Count != 0 && (googleChatResponses[0].Candidates == null && googleChatResponses[0].PromptFeedback != null))
                    {
                        var contentFilterMessage = HandleContentFilteredMessage(googleChatResponses[0]);
                        assistantMessage.Append(contentFilterMessage);
                        break;
                    }

                    var textParts = googleChatResponses
                        .SelectMany(message => message.Candidates)
                        .SelectMany(candidate => candidate.Content.Parts)
                        .OfType<GoogleTextContentPart>(); // Filters parts, keeping only those of type GoogleTextContentPart

                    foreach (var textPart in textParts)
                    {
                        assistantMessage.Append(textPart.Text);
                    }
                }

                // Gemini doesn't publish messages as they are generated, so we publish the final message here
                _eventAggregator.PublishMessageReceived(new MessageReceivedEvent
                {
                    Message = new AnthropicChatResponseDto
                    {
                        Content = assistantMessage.ToString(),
                        Role = "model"
                    }
                });

                return new GoogleChatResponseDto { Content = assistantMessage.ToString(), Role = "model" };
            }
        }

        private GoogleGenerateContentRequest CreateRequestBody(GoogleChatRequest chatRequest)
        {
            var apiMessages = chatRequest.Messages.Select(message => new GoogleBaseChatMessageDto
            {
                Role = message.Role,
                Parts = message.Content
            }).ToList();

            return new GoogleGenerateContentRequest
            {
                Contents = apiMessages,
                GenerationConfig = new GoogleChatGenerationConfig
                {
                    MaxTokens = chatRequest.Settings.MaxTokens,
                    Temperature = chatRequest.Settings.Temperature
                }
            };
        }

        private (string Endpoint, string ModelMethod) CreateEndpoint(GoogleChatRequest chatRequest)
        {
            var googleApiKey = _appSettingsService.GoogleAIApiKey != null && _appSettingsService.GoogleAIApiKey != "" ?
                _appSettingsService.GoogleAIApiKey :
                _userSettingsService.Get<string>(UserSettings.GoogleAIApiKey);

            var modelMethod = (chatRequest.Settings.Stream) ? "streamGenerateContent" : "generateContent";
            var endpoint = $"v1beta/models/{chatRequest.Model}:{modelMethod}?key={googleApiKey}";
            return (endpoint, modelMethod);
        }

        private async Task<HttpResponseMessage> SendMessagesRequest(GoogleGenerateContentRequest body, string endpoint, CancellationTokenSource cancellationTokenSource)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, cancellationTokenSource.Token);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send request to Google Gemini API. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            }
            
            return response;
        }

        private string HandleContentFilteredMessage(GoogleGenerateContentResponse googleChatResponse)
        {
            if (!Enum.TryParse(googleChatResponse.PromptFeedback.BlockReason, out BlockReason blockReason))
            {
                return "Prompt was blocked due to unknown reasons.";
            }

            var safetyRatings = googleChatResponse.PromptFeedback.SafetyRatings;
            var blockReasonString = blockReasons[blockReason];
            var safetyRatingsString = safetyRatings.Select(rating => $"{rating.Category}: {rating.Probability}");

            var message = $"Prompt flagged by Gemini:\n\n{blockReasonString}\n\nSafety ratings:\n{string.Join("\n", safetyRatingsString)}";

            return message;
        }

        public ModelSetting GetModelSetting(string key)
        {
            if (modelSettings.ContainsKey(key))
            {
                return modelSettings[key];
            }

            return null;
        }

        public bool ValidateApiKey()
        {
            string googleApiKey = _appSettingsService.GoogleAIApiKey != null && _appSettingsService.GoogleAIApiKey != "" ?
                _appSettingsService.GoogleAIApiKey :
                _userSettingsService.Get<string>(UserSettings.GoogleAIApiKey);

            return !string.IsNullOrEmpty(googleApiKey);
        }
    }
}