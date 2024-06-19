using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IMessageMapper
    {
        ChatMessageDto MapToDto(ChatMessage assistant);
        ChatMessage MapToEntity(ChatMessageDto dto);
        void UpdateEntityFromDto(ChatMessageDto dto, ChatMessage entity);
    }
}
