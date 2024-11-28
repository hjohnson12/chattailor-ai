using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Tools
{
    public class Tool
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}