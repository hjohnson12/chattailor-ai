using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Prompts
{
    public class Prompt
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public PromptType PromptType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Prompt()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
