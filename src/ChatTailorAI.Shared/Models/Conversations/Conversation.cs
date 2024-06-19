using ChatTailorAI.Shared.Models.Assistants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Conversations
{
    public class Conversation
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ConversationType { get; set; }
        public string Instructions { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Conversation()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            ConversationType = "Standard";
            IsDeleted = false;
        }
    }
}