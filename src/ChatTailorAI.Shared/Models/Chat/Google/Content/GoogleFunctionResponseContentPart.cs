using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Google.Content
{
    public class GoogleFunctionResponseContentPart : IGoogleChatContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("response")]
        public Dictionary<string, object> Response { get; set; }

        public GoogleFunctionResponseContentPart()
        {
            Response = new Dictionary<string, object>();
        }
    }
}
