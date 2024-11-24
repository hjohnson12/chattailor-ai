using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatTailorAI.Shared.Models.Speech
{
    public class WhisperResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}