using System;
using ChatTailorAI.Shared.Models.Prompts;

namespace ChatTailorAI.Shared.Models.Chat
{
    public class ChatImage
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public virtual ChatImageMessage ChatImageMessage { get; set; }
        public string PromptId { get; set; }
        public virtual Prompt Prompt { get; set; }  // Navigation property
        public string ModelIdentifier { get; set; }
        public string Url { get; set; }
        public string Size { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ChatImage()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}