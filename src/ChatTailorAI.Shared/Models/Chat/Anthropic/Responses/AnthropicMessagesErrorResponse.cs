using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Responses
{
    public class AnthropicMessagesErrorResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("error")]
        public ErrorDetails Error { get; set; }

        public class ErrorDetails
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
}