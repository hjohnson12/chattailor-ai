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
    public class ElevenLabsVoiceRequest
    {
        public string Text { get; set; }
        public string ModelId { get; set; }
        public ElevenLabsVoiceSettings VoiceSettings { get; set; }
    }

    public class ElevenLabsVoiceSettings
    {
        public double Stability { get; set; }
        public double SimilarityBoost { get; set; }
    }

    public class ElevenLabsSpeechService : IElevenLabsSpeechService
    {
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private static HttpClient _httpClient;
        private string[] models;
        private string[] voices;

        private readonly string elevenLabsApiUrl = "https://api.elevenlabs.io/";

        public ElevenLabsSpeechService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;

            // TODO: Update with ones from ElevenLabs apis

            this.models = new string[] { "eleven_multilingual_v2" };
            this.voices = new string[] { };

            var elevenLabsApiKey = _appSettingsService.ElevenLabsApiKey != null && _appSettingsService.ElevenLabsApiKey != "" ?
                _appSettingsService.ElevenLabsApiKey :
                _userSettingsService.Get<string>(UserSettings.ElevenLabsApiKey);

            client = new HttpClient { Timeout = TimeSpan.FromMinutes(4) };
            client.BaseAddress = new Uri(elevenLabsApiUrl);
            client.Timeout = TimeSpan.FromMinutes(4);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("xi-api-key", $"{elevenLabsApiKey}");
            _httpClient = client;
        }

        public async Task<Stream> SynthesizeSpeechAsync(string text)
        {
            var apiKey = _userSettingsService.Get<string>(UserSettings.ElevenLabsApiKey);
            if (apiKey == null)
            {
                throw new Exception("ElevenLabs API key not found.");
            }

            var voice = _userSettingsService.Get<string>(UserSettings.VoiceName);
            var voiceSettings = new Dictionary<string, object>
            {
                { "stability", 0.5 },
                { "similarity_boost", 0.5 }
            };

            var requestBody = new Dictionary<string, object>
            {
                { "text", text },
                { "model_id", "eleven_multilingual_v2" },
                { "voice_settings", voiceSettings }
            };

            var voiceId = await this.GetVoiceIdAsync(voice);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/text-to-speech/{voiceId}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
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

        public async Task<List<dynamic>> GetVoicesListAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"v1/voices");

            using (var response = await _httpClient.SendAsync(httpRequestMessage))
            {
                // TODO: Add better error handling, silent error from this
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                // TODO: Add response type
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                var voicesList = new List<dynamic>();

                foreach (var voice in responseObject.voices)
                {
                    voicesList.Add(voice);
                }

                return voicesList;
            }
        }

        public async Task<string> GetVoiceIdAsync(string voiceName)
        {
            try
            {
                var voicesList = await GetVoicesListAsync();

                foreach (var voice in voicesList)
                {
                    if (voice.name == voiceName)
                    {
                        return voice.voice_id;
                    }
                }

                throw new InvalidOperationException($"Voice with name '{voiceName}' not found.");
            }
            catch (HttpRequestException httpEx)
            {
                throw new InvalidOperationException($"Error while getting voiceId of selected voice: {httpEx.Message}", httpEx);
            }
            catch (JsonException jsonEx)
            {
                throw new InvalidOperationException("Error while parsing JSON response.", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting voiceId of selected voice: {ex.Message}", ex);
            }
        }

        public Task<List<string>> GetModelsListAsync()
        {
            return Task.FromResult(new List<string>(this.models));
        }

        public async Task<List<string>> GetVoiceNamesAsync()
        {
            // TODO: Add error handling
            var voicesList = await this.GetVoicesListAsync();
            var namesList = new List<string>();

            foreach (var voice in voicesList)
            {
                string name = voice.name;  
                namesList.Add(name);
            }

            return namesList;
        }
    }
}