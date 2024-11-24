using System;
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
    public class OpenAIRunService : IOpenAIRunService
    {
        private static HttpClient _httpClient;
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private IEventAggregator _eventAggregator;

        public OpenAIRunService(
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

        public async Task CreateRunAsync()
        {

        }

        public async Task<OpenAIThreadRun> CreateRunAsync(string assistantId, string threadId)
        {
            var runData = new
            {
                assistant_id = assistantId
            };

            var jsonContent = JsonConvert.SerializeObject(runData);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"v1/threads/{threadId}/runs", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error creating run: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var run = JsonConvert.DeserializeObject<OpenAIThreadRun>(responseContent);
            return run;
        }

        public async Task<OpenAIThreadRun> RetrieveRunAsync(string runId, string threadId)
        {
            var response = await _httpClient.GetAsync($"v1/threads/{threadId}/runs/{runId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error retrieving run: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var run = JsonConvert.DeserializeObject<OpenAIThreadRun>(responseContent);
            return run;
        }

        public async Task ModifyRunAsync()
        {

        }

        public async Task ListRunsAsync()
        {

        }

        public async Task SubmitToolOutputsToRunAsync()
        {

        }

        public async Task CancelRun()
        {

        }

        public async Task CreateThreadAndRunAsync()
        {

        }

        public async Task RetrieveRunStepAsync()
        {

        }

        public async Task ListRunStepsAsync()
        {

        }
    }
}
