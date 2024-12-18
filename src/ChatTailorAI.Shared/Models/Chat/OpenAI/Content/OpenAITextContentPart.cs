﻿using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Chat.OpenAI.Content
{
    public class OpenAITextContentPart : IOpenAIChatContent
    {
        [JsonProperty("type")]
        public string Type => "text";

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}