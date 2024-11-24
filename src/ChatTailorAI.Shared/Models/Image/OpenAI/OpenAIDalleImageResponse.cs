using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Image.OpenAI
{
    public class OpenAIDalleImageResponse
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("data")]
        public List<ImageData> Data { get; set; }
    }

    public class ImageData
    {
        public string Url { get; set; }
    }
}
