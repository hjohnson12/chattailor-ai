using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
