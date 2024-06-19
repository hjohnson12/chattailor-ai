using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Google.Content
{
    public class GoogleFunctionCallContentPart : IGoogleChatContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("args")]
        public Dictionary<string, object> Args { get; set; }

        public GoogleFunctionCallContentPart()
        {
            Args = new Dictionary<string, object>();
        }
    }
}