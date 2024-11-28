using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Dto.Chat.Anthropic
{
    public class AnthropicBaseChatMessageDto : IChatModelMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }
    }

    public class AnthropicUserChatMessageDto : AnthropicBaseChatMessageDto
    {
        
    }

    public class AnthropicAssistantChatMessageDto : AnthropicBaseChatMessageDto
    {
        
    }
}