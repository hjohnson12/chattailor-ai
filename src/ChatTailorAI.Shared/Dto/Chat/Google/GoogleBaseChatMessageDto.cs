using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Dto.Chat.Google
{
    public class GoogleBaseChatMessageDto : IChatModelMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public object Content { get; set; }

        [JsonProperty("parts", NullValueHandling = NullValueHandling.Ignore)]
        public object Parts { get; set; }
    }

    public class GoogleUserChatMessageDto : GoogleBaseChatMessageDto
    {

    }

    public class GoogleAssistantChatMessageDto : GoogleBaseChatMessageDto
    {

    }
}