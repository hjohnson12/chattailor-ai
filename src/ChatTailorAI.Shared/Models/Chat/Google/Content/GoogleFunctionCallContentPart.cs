using System.Collections.Generic;
using Newtonsoft.Json;

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