﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Google.Content
{
    public class GoogleImageContentPart : IGoogleChatContent
    {
        [JsonProperty("inlineData")]
        public InlineDataDetails InlineData { get; set; }

        public class InlineDataDetails
        {
            [JsonProperty("data")]
            public string Data { get; set; }

            [JsonProperty("mimeType")]
            public string MimeType { get; set; }
        }
    }
}
