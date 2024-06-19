using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IOpenAIChatRequestMapper
    {
        OpenAIChatRequestDto MapToDto(OpenAIChatRequest request);
    }
}
