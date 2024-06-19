using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Google.Content
{
    public class GoogleTextContentPart : IGoogleChatContent
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}