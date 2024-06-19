using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.LMStudio.Responses
{
    public class LMStudioChatErrorResponse
    {
        [JsonProperty("error")]
        public ErrorDetails Error { get; set; }
    }

    public class ErrorDetails
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("param")]
        public string Param { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
