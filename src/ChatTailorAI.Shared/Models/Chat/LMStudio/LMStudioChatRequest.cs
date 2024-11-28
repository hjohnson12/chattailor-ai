using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using System.Collections.Generic;

namespace ChatTailorAI.Shared.Models.Chat.LMStudio
{
    public class LMStudioChatRequest : ChatRequest<LMStudioChatSettings, LMStudioBaseChatMessageDto>
    {
        public LMStudioChatRequest() { }

        public LMStudioChatRequest(List<LMStudioBaseChatMessageDto> messages, LMStudioChatSettings chatSettings)
        {
            Messages = messages;
            Settings = chatSettings;
        }
    }
}