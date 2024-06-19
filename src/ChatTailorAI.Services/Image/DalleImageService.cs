using ChatTailorAI.Services.Events;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Events.EventArgs;
using ChatTailorAI.Shared.Models.Image.OpenAI;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Services.Image;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ChatTailorAI.Services.Image
{
    public class DalleImageService : IDalleImageService
    {
        private static HttpClient _httpClient;
        private readonly IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private readonly IEventAggregator _eventAggregator;
        private string[] models;
        private Dictionary<string, List<string>> modelImageSizes;

        private readonly string dalleBaseUrl = "https://api.openai.com/";

        public DalleImageService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            IEventAggregator eventAggregator,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;
            _eventAggregator = eventAggregator;

            models = new string[2] { "dall-e-3", "dall-e-2" };
            modelImageSizes = new Dictionary<string, List<string>>()
            {
                ["dall-e-3"] = new List<string> { "1024x1024", "1792x1024", "1024x1792" },
                ["dall-e-2"] = new List<string> { "256x256", "512x512", "1024x1024" }
            };

            _eventAggregator.ApiKeyChanged += OnApiKeyChanged;

            var openAiApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            client = new HttpClient { Timeout = TimeSpan.FromMinutes(4) };
            client.BaseAddress = new Uri(dalleBaseUrl);
            client.Timeout = TimeSpan.FromMinutes(4);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
            _httpClient = client;
        }

        private void OnApiKeyChanged(object sender, ApiKeyChangedEventArgs e)
        {
            if (e.KeyType == ApiKeyType.OpenAI)
            {
                UpdateAuthorizationHeader(e.ApiKey);
            }
        }

        public async Task<IEnumerable<string>> GenerateImagesAsync(OpenAIImageGenerationRequest imageRequest)
        {
            var dalleImageRequest = new OpenAIDalleImageRequest()
            {
                Model = imageRequest.Settings.Model,
                Prompt = imageRequest.Settings.Prompt,
                N = imageRequest.Settings.N,
                Size = imageRequest.Settings.Size,
                ResponseFormat = "url",
                ImageQuality = imageRequest.Settings.ImageQuality
            };

            using (var response = await PostRequest(dalleImageRequest, "v1/images/generations"))
            {
                var str = await response.Content.ReadAsStringAsync();
                var imageResponse = JsonConvert.DeserializeObject<OpenAIDalleImageResponse>(str);
                return imageResponse.Data.Select(img => img.Url).ToList();
            }
        }

        private async Task<HttpResponseMessage> PostRequest(OpenAIDalleImageRequest body, string endpoint)
        {
            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public void UpdateAuthorizationHeader(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }
    }
}