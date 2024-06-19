using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Tools
{
    public class Tool
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
