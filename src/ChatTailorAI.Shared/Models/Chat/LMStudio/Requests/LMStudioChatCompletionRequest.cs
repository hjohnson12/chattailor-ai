using System.Collections.Generic;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Dto.Chat.LMStudio;

namespace ChatTailorAI.Shared.Models.Chat.LMStudio.Requests
{
    public class LMStudioChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("messages")]
        public List<LMStudioBaseChatMessageDto> Messages { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("stream")]
        public bool Stream { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }
    }
}