using System.Collections.Generic;
using ChatTailorAI.Shared.Dto.Chat.Google;

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