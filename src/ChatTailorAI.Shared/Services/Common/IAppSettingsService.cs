using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IAppSettingsService
    {
        string OpenAiApiKey { get; }
        string SpotifyClientId { get; }
        string SpotifyClientSecret { get; }
        string SpotifyApiKey { get; }
        string ElevenLabsApiKey { get; }
        string GoogleAIApiKey { get; }
        string AnthropicApiKey { get; }
        string LMStudioServerUrl { get; }
    }
}
