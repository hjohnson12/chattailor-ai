using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.Assistants.OpenAI
{
    public class OpenAIThreadService : IOpenAIThreadService
    {
        private static HttpClient _httpClient;
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private IEventAggregator _eventAggregator;

        public OpenAIThreadService(
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

        public async Task<OpenAIThread> CreateThreadAsync(string[] messages = null)
        {
            var threadData = new
            {
                messages = messages ?? Array.Empty<string>()
            };

            var jsonContent = JsonConvert.SerializeObject(threadData);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("v1/threads", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error creating thread: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenAIThread>(responseContent);
        }

        public async Task RetrieveThreadAsync()
        {

        }

        public async Task ModifyThreadAsync()
        {

        }

        public async Task DeleteThreadAsync(string threadId)
        {
            var response = await _httpClient.DeleteAsync($"v1/threads/{threadId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error deleting thread: {response.StatusCode}");
            }

            // TODO: parse output obj and return status

        }
    }
}
