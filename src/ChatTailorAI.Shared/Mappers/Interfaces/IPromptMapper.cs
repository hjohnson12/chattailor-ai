using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Prompts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IPromptMapper
    {
        PromptDto MapToDto(Prompt prompt);
        Prompt MapToEntity(PromptDto promptDto);
        void UpdateEntityFromDto(PromptDto promptDto, Prompt prompt);
    }
}
