using ChatTailorAI.Shared.Models.Chat.OpenAI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat.OpenAI
{
    public class OpenAIChatRequestDto : ChatRequestDto<OpenAIChatSettingsDto, OpenAIBaseChatMessageDto>
    {
        // Inherit from ChatRequestDto
    }
}