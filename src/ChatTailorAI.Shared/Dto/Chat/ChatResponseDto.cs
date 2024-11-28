using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatResponseDto
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}