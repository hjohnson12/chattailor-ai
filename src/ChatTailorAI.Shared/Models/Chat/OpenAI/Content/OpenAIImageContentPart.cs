using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.OpenAI.Content
{
    public class OpenAIImageContentPart : IOpenAIChatContent
    {
        [JsonProperty("type")]
        public string Type => "image_url";

        [JsonProperty("image_url")]
        public ImageUrlDetails ImageUrl { get; set; }

        public class ImageUrlDetails
        {
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("detail")]
            public string Detail { get; set; }
        }
    }
}