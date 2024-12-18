﻿using System.Collections.Generic;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Dto.Chat.Anthropic;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Requests
{
    public class AnthropicMessagesRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("messages")]
        public List<AnthropicBaseChatMessageDto> Messages { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }


        [JsonProperty("stop", NullValueHandling = NullValueHandling.Ignore)]
        public string Stop { get; set; }

        [JsonProperty("stream")]
        public bool Stream { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("top_p")]
        public double TopP { get; set; }

        [JsonProperty("top_k")]
        public int TopK { get; set; }
    }
}