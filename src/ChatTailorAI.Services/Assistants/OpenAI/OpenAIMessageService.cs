using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;

namespace ChatTailorAI.Services.Assistants.OpenAI
{
    public class OpenAIMessageService : IOpenAIMessageService
    {
        private static HttpClient _httpClient;
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private IEventAggregator _eventAggregator;

        public OpenAIMessageService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            IEventAggregator eventAggregator,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;
            _eventAggregator = eventAggregator;

            var openAiApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            _eventAggregator.ApiKeyChanged += OnApiKeyChanged;

            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
            client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
            _httpClient = client;
        }

        private void OnApiKeyChanged(object sender, Shared.Events.EventArgs.ApiKeyChangedEventArgs e)
        {
            if (e.KeyType == Shared.Events.ApiKeyType.OpenAI)
            {
                UpdateAuthorizationHeader(e.ApiKey);
            }
        }

        public void UpdateAuthorizationHeader(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<OpenAIThreadMessage> CreateMessageAsync(string threadId, string message)
        {
            var messageData = new
            {
                role = "user",
                content = message
            };

            var jsonContent = JsonConvert.SerializeObject(messageData);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"v1/threads/{threadId}/messages", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error creating message: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenAIThreadMessage>(responseContent);
        }

        public async Task RetrieveMessageAsync()
        {

        }

        public async Task ModifyMessageAsync()
        {

        }

        public async Task<List<OpenAIThreadMessage>> ListMessagesAsync(string threadId)
        {
            var response = await _httpClient.GetAsync($"v1/threads/{threadId}/messages");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error listing messages: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<OpenAIThreadMessagesResponse>(responseContent);
            var messages = responseObj.Data;

             return messages;
        }

        public async Task RetrieveMessageFileAsync()
        {

        }

        public async Task ListMessageFilesAsync()
        {

        }
    }
}
