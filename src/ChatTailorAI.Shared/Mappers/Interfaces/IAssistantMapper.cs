using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IAssistantMapper
    {
        AssistantDto MapToDto(Assistant assistant);
        Assistant MapToEntity(AssistantDto dto);
        void UpdateEntityFromDto(AssistantDto dto, Assistant entity);
    }
}