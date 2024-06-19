using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Events
{
    public enum ApiKeyType
    {
        AzureSpeech,
        OpenAI,
        ElevenLabs,
        Anthropic,
        Google
    }

    public class ApiKeyUpdatedEvent
    {
        public ApiKeyType KeyType { get; }
        public string NewApiKey { get; }

        public ApiKeyUpdatedEvent(ApiKeyType keyType, string newApiKey)
        {
            KeyType = keyType;
            NewApiKey = newApiKey;
        }
    }
}