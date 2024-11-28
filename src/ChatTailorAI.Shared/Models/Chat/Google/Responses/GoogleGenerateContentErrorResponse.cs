using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Chat.Google.Responses
{
    public class GoogleGenerateContentErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("details")]
        public List<Detail> Details { get; set; }
    }

    public class Detail
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("fieldViolations")]
        public List<FieldViolation> FieldViolations { get; set; }
    }

    public class FieldViolation
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

}
