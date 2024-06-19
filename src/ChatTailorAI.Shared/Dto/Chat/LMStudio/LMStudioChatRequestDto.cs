using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat.LMStudio
{
    public class LMStudioChatRequestDto : ChatRequestDto<LMStudioChatSettingsDto, LMStudioBaseChatMessageDto>
    {
        // Inherit from ChatRequestDto
    }
}