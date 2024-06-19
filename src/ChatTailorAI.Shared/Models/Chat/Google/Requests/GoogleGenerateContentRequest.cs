using ChatTailorAI.Shared.Dto.Chat.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Google.Requests
{
    public class GoogleGenerateContentRequest
    {
        [JsonProperty("contents")]
        public List<GoogleBaseChatMessageDto> Contents { get; set; }

        [JsonProperty("generationConfig")]
        public GoogleChatGenerationConfig GenerationConfig { get; set; }
    }

    public class GoogleChatGenerationConfig
    {
        [JsonProperty("maxOutputTokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }
    }
}
