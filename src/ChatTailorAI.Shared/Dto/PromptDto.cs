using ChatTailorAI.Shared.Models.Prompts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto
{
    public class PromptDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public bool IsActive { get; set; }
        public PromptType PromptType { get; set; }
        public DateTime CreatedAt { get; set; }

        public PromptDto()
        { 
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

        public PromptDto(Prompt prompt)
        {
            Id = prompt.Id;
            Title = prompt.Title;
            Content = prompt.Content;
            IsActive = prompt.IsActive;
            PromptType = prompt.PromptType;
            CreatedAt = prompt.CreatedAt;
        }
    }
}