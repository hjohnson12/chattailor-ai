using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Models.Image.OpenAI
{
    public class OpenAIDalleImageRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("response_format")]
        public string ResponseFormat { get; set; }

        [JsonProperty("quality", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageQuality { get; set; }
    }
}