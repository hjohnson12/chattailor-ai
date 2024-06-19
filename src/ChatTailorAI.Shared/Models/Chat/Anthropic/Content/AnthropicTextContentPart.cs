using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Content
{
    public class AnthropicTextContentPart : IAnthropicChatContent
    {
        [JsonProperty("type")]
        public string Type => "text";

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}