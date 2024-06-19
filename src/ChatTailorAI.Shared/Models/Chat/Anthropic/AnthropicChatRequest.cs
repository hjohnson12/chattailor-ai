using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using System;
using System.Collections.Generic;
using System.Text;

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