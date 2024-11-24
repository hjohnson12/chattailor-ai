using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Prompts;

namespace ChatTailorAI.Shared.Mappers
{
    public class PromptMapper : IPromptMapper
    {
        public PromptDto MapToDto(Prompt prompt)
        {
            return new PromptDto
            {
                Id = prompt.Id,
                Title = prompt.Title,
                Content = prompt.Content,
                IsActive = prompt.IsActive,
                PromptType = prompt.PromptType,
                CreatedAt = prompt.CreatedAt,
            };
        }

        public Prompt MapToEntity(PromptDto promptDto)
        {
            return new Prompt
            {
                Id = promptDto.Id,
                Title = promptDto.Title,
                Content = promptDto.Content,
                IsActive = promptDto.IsActive,
                PromptType = promptDto.PromptType,
                CreatedAt = promptDto.CreatedAt,
            };
        }

        public void UpdateEntityFromDto(PromptDto promptDto, Prompt entity)
        {
            entity.Id = promptDto.Id;
            entity.Title = promptDto.Title;
            entity.Content = promptDto.Content;
            entity.PromptType = promptDto.PromptType;
            entity.CreatedAt = promptDto.CreatedAt;

            entity.IsActive = promptDto.IsActive;
        }
    }
}