using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Speech;

namespace ChatTailorAI.Services.Speech
{
    public class OpenAISpeechService : IOpenAISpeechService
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserSettingsService _userSettingsService;
        private static HttpClient _httpClient;
        private readonly string[] models;
        private readonly string[] voices;

        private readonly string openAISpeechApiUrl = "https://api.openai.com/";

        public OpenAISpeechService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;

            this.models = new string[] { "tts-1", "tts-1-hd" };
            this.voices = new string[] { "alloy", "echo", "fable", "onyx", "nova", "shimmer" };
            
            var openAiApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            client = new HttpClient { Timeout = TimeSpan.FromMinutes(4) };
            client.BaseAddress = new Uri(openAISpeechApiUrl);
            client.Timeout = TimeSpan.FromMinutes(4);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
            _httpClient = client;
        }

        public Task<List<string>> GetVoicesListAsync()
        {
            return Task.FromResult(new List<string>(this.voices));
        }

        public async Task<Stream> SynthesizeSpeechAsync(string text)
        {
            var apiKey = _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);
            if (apiKey == null)
            {
                throw new Exception("OpenAI API Key is not set.");
            }

            var voice = _userSettingsService.Get<string>(UserSettings.VoiceName);
            var request = new
            {
                model = "tts-1-hd",
                input = text,
                voice = voice
            };

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "v1/audio/speech")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            using (var response = await _httpClient.SendAsync(httpRequestMessage))
            {
                response.EnsureSuccessStatusCode();

                var memoryStream = new MemoryStream();
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    await responseStream.CopyToAsync(memoryStream);
                }

                memoryStream.Position = 0;

                return memoryStream;
            }
        }

        public Task<List<string>> GetModelsListAsync()
        {
            return Task.FromResult(new List<string>(this.models));
        }
    }
}
