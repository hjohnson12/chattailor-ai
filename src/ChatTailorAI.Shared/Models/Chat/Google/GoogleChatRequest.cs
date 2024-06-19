using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.Anthropic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Google
{
    public class GoogleChatRequest : ChatRequest<GoogleChatSettings, GoogleBaseChatMessageDto>
    {
        public GoogleChatRequest() { }

        public GoogleChatRequest(List<GoogleBaseChatMessageDto> messages, GoogleChatSettings chatSettings)
        {
            Messages = messages;
            Settings = chatSettings;
        }
    }
}