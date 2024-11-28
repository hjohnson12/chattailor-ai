using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Models.Speech;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Speech;

namespace ChatTailorAI.Services.Speech
{
    public class AzureSpeechService : IAzureSpeechService
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly string[] models;
        private string _voiceName;

        public AzureSpeechService(
            IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;

            _voiceName = _userSettingsService.Get<string>(UserSettings.VoiceName);
            models = new string[] { "default" };
        }

        public string ServiceRegion
        {
            get => _userSettingsService.Get<string>(UserSettings.SpeechServiceRegion);
            set => _userSettingsService.Set(UserSettings.SpeechServiceRegion, value);
        }

        public async Task SynthesizeSpeechAsync(string text)
        {
            var azureSpeechApiKey = _userSettingsService.Get<string>(UserSettings.AzureSpeechServicesKey);
            if (azureSpeechApiKey == null || azureSpeechApiKey.Equals(""))
            {
                return;
            }


            var config = SpeechConfig.FromSubscription(azureSpeechApiKey, ServiceRegion);
            _voiceName = _userSettingsService.Get<string>(UserSettings.VoiceName);
            config.SpeechSynthesisVoiceName = _voiceName.Replace(" (Male)", "").Replace(" (Female)", "");

            using (var synthesizer = new SpeechSynthesizer(config, AudioConfig.FromDefaultSpeakerOutput()))
            {
                var result = await synthesizer.SpeakTextAsync(text);
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                }
            }
        }

        public async Task<List<string>> GetVoicesListAsync()
        {
            var azureSpeechApiKey = _userSettingsService.Get<string>(UserSettings.AzureSpeechServicesKey);
            var voiceListUrl = $"https://{ServiceRegion.ToLower()}.tts.speech.microsoft.com/cognitiveservices/voices/list";

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(voiceListUrl),
                Headers =
                {
                    { "Ocp-Apim-Subscription-Key", azureSpeechApiKey },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var speechModels = JsonConvert.DeserializeObject<List<AzureSpeechModel>>(body);

                return speechModels
                    .Select(sm => $"{sm.ShortName} ({sm.Gender})")
                    .ToList();
            }
        }

        public Task<List<string>> GetModelsListAsync()
        {
            return Task.FromResult(new List<string>(models));
        }
    }
}