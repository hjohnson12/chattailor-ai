using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Content
{
    public class AnthropicImageContentPart : IAnthropicChatContent
    {
        [JsonProperty("type")]
        public string Type => "image";

        [JsonProperty("source")]
        public ImageSourceDetails Source { get; set; }

        public class ImageSourceDetails
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("media_type")]
            public string MediaType { get; set; }

            [JsonProperty("data")]
            public string Data { get; set; }
        }
    }
}