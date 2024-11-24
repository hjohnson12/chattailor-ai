using System.Collections.Generic;
using ChatTailorAI.Shared.Dto.Chat.Anthropic;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic
{
    public class AnthropicChatRequest : ChatRequest<AnthropicChatSettings, AnthropicBaseChatMessageDto>
    {
        public AnthropicChatRequest() { }

        public AnthropicChatRequest(List<AnthropicBaseChatMessageDto> messages, AnthropicChatSettings chatSettings)
        {
            Messages = messages;
            Settings = chatSettings;
        }
    }
}