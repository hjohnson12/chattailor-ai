using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Assistants;

namespace ChatTailorAI.Shared.Mappers
{
    public class AssistantMapper : IAssistantMapper
    {
        public AssistantDto MapToDto(Assistant assistant)
        {
            return new AssistantDto
            {
                Id = assistant.Id,
                Name = assistant.Name,
                ExternalAssistantId = assistant.ExternalAssistantId,
                Model = assistant.Model,
                Description = assistant.Description,
                CreatedAt = assistant.CreatedAt,
                Instructions = assistant.Instructions,
                AssistantType = assistant.AssistantType
            };
        }

        public Assistant MapToEntity(AssistantDto dto)
        {
            return new Assistant
            {
                Id = dto.Id,
                Name = dto.Name,
                ExternalAssistantId = dto.ExternalAssistantId,
                Description = dto.Description,
                Model = dto.Model,
                CreatedAt = dto.CreatedAt,
                Instructions = dto.Instructions,
                AssistantType = dto.AssistantType
            };
        }

        public void UpdateEntityFromDto(AssistantDto dto, Assistant entity)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.ExternalAssistantId = dto.ExternalAssistantId;
            entity.Instructions = dto.Instructions;
            entity.Model = dto.Model;
            entity.CreatedAt = dto.CreatedAt;
            entity.AssistantType = dto.AssistantType;
        }
    }
}
