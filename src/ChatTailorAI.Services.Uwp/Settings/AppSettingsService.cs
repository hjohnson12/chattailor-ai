using Windows.ApplicationModel.Resources;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Services.Uwp.Settings
{
    public class AppSettingsService : IAppSettingsService
    {
        public AppSettingsService()
        {
            var resourceLoader = ResourceLoader.GetForCurrentView("appsettings-admin");
            OpenAiApiKey = resourceLoader.GetString(nameof(OpenAiApiKey));
            SpotifyClientId = resourceLoader.GetString(nameof(SpotifyClientId));
            SpotifyClientSecret = resourceLoader.GetString(nameof(SpotifyClientSecret));
            SpotifyApiKey = resourceLoader.GetString(nameof(SpotifyApiKey));
            ElevenLabsApiKey = resourceLoader.GetString(nameof(ElevenLabsApiKey));
            GoogleAIApiKey = resourceLoader.GetString(nameof(GoogleAIApiKey));
            AnthropicApiKey = resourceLoader.GetString(nameof(AnthropicApiKey));
            LMStudioServerUrl = resourceLoader.GetString(nameof(LMStudioServerUrl));
        }

        public string OpenAiApiKey { get; }
        public string SpotifyClientId { get; }
        public string SpotifyClientSecret { get; }
        public string SpotifyApiKey { get; }
        public string ElevenLabsApiKey { get; }
        public string GoogleAIApiKey { get; }
        public string AnthropicApiKey { get; }
        public string LMStudioServerUrl { get; }
    }
}