using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
