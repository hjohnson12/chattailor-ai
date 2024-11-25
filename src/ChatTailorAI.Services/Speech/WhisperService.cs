using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Speech;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Speech;
using ChatTailorAI.Shared.Models.Settings;

namespace ChatTailorAI.Services.Speech
{
    public class WhisperService : IWhisperService
    {
        private readonly string whisperApiUrl = "https://api.openai.com/";
        private readonly string TRANSCRIPTION_API_ENDPOINT = "v1/audio/transcriptions";
        private readonly string TRANSLATION_API_ENDPOINT = "v1/audio/translations";
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserSettingsService _userSettingsService;
        private static HttpClient _httpClient;

        public WhisperService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;

            var apiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            client.BaseAddress = new Uri(whisperApiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _httpClient = client;
        }

        public async Task<string> Transcribe(string filename, byte[] audioBuffer)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(audioBuffer)), "file", filename);
                    content.Add(new StringContent("whisper-1"), "model");

                    var response = await _httpClient.PostAsync(TRANSCRIPTION_API_ENDPOINT, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                        return $"Non 200 response from transcription: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error transcribing audio", ex);
            }
        }

        public async Task<string> Translate(string filename, byte[] audioBuffer)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(audioBuffer)), "file", filename);
                    content.Add(new StringContent("whisper-1"), "model");

                    var response = await _httpClient.PostAsync(TRANSLATION_API_ENDPOINT, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var text = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<WhisperResponse>(text).Text;
                    }
                    else
                    {
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                        return $"Non 200 response from translation: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error transcribing audio", ex);
            }
        }

        public async Task<byte[]> StreamToBuffer(Stream stream)
        {
            Console.WriteLine("Converting stream to buffer...");

            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}