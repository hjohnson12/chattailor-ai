using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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