using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Models.Speech
{
    public class AzureSpeechModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("LocalName")]
        public string LocalName { get; set; }

        [JsonProperty("ShortName")]
        public string ShortName { get; set; }

        [JsonProperty("Gender")]
        public string Gender { get; set; }

        [JsonProperty("Locale")]
        public string Locale { get; set; }

        [JsonProperty("LocaleName")]
        public string LocaleName { get; set; }

        [JsonProperty("SampleRateHertz")]
        public string SampleRateHertz { get; set; }

        [JsonProperty("VoiceType")]
        public string VoiceType { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("WordsPerMinute")]
        public string WordsPerMinute { get; set; }
    }
}
