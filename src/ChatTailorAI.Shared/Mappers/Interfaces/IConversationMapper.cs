using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Models.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IConversationMapper
    {
        ConversationDto MapToDto(Conversation conversation);
        Conversation MapToEntity(ConversationDto dto);
        void UpdateEntityFromDto(ConversationDto dto, Conversation entity);
    }
}
