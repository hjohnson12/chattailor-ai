using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Speech
{
    public class WhisperResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
