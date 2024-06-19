using System.Collections.Generic;

namespace ChatTailorAI.Shared.Models.Settings
{
    public class UserSettings
    {
        public const string OpenAiApiKey = "OpenAiApiKey";
        public const string AzureSpeechServicesKey = "AzureSpeechServicesKey";
        public const string ElevenLabsApiKey = "ElevenLabsApiKey";
        public const string GoogleAIApiKey = "GoogleAIApiKey";
        public const string AnthropicApiKey = "AnthropicApiKey";
        public const string LMStudioServerUrl = "LMStudioServerUrl";

        // Chat Settings
        public const string StreamReply = "StreamReply";
        public const string ChatModel = "ChatModel";
        public const string MaxTokens = "MaxTokens";
        public const string Temperature = "Temperature";
        public const string FrequencyPenalty = "FrequencyPenalty";
        public const string PresencePenalty = "PresencePenalty";
        public const string SystemMessage = "SystemMessage";
        public const string DefaultPromptId = "DefaultPromptId";

        // Image Settings
        public const string ImageModel = "ImageModel";
        public const string ImageCount = "ImageCount";
        public const string ImageSize = "ImageSize";
        public const string ImageQuality = "ImageQuality";

        // Speech Settings
        public const string VoiceName = "VoiceName";
        public const string SpeechEnabled = "SpeechEnabled";
        public const string SpeechProvider = "SpeechProvider"; // "Azure" or "OpenAI" or "ElevenLabs"
        public const string SpeechModel = "SpeechModel";
        public const string SpeechToTextEnabled = "SpeechToTextEnabled";
        public const string SpeechServiceRegion = "SpeechServiceRegion";
        public const string AzureSpeechEndpoint = "AzureSpeechEndpoint";

        // Function Settings
        public const string FunctionsEnabled = "FunctionsEnabled";
        public const string FunctionsSelected = "FunctionSelected";

        public static IReadOnlyDictionary<string, object> Defaults { get; } = new Dictionary<string, object>()
        {
            { OpenAiApiKey, "" },
            { AzureSpeechServicesKey, "" },
            { ElevenLabsApiKey, "" },
            { GoogleAIApiKey, "" },
            { AnthropicApiKey, "" },
            { LMStudioServerUrl, "" },
            // Chat
            { StreamReply, true },
            { ChatModel, "gpt-4o" },
            { MaxTokens, 2000 },
            { Temperature, 0.9 },
            { FrequencyPenalty, 0.0 },
            { PresencePenalty, 0.6 },
            {
                SystemMessage,
                null
            },
            {
                DefaultPromptId,
                null
            },
            // Image
            { ImageModel, "dall-e-3" },
            { ImageCount, "1" },
            { ImageSize, "256x256" },
            { ImageQuality, "standard" },
            // Speech
            { AzureSpeechEndpoint, "" },
            { SpeechProvider, "openai" },
            { SpeechModel, "tts-1" },
            { VoiceName, "alloy" },
            { SpeechEnabled, false },
            { SpeechToTextEnabled, false },
            // Functions
            { FunctionsEnabled, false },
            { FunctionsSelected, "" },
            { SpeechServiceRegion, "" },
        };
    }
}