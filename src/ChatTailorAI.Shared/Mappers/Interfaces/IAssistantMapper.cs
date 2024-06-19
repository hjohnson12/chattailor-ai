using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IAssistantMapper
    {
        AssistantDto MapToDto(Assistant assistant);
        Assistant MapToEntity(AssistantDto dto);
        void UpdateEntityFromDto(AssistantDto dto, Assistant entity);
    }
}
