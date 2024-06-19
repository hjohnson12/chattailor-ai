using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat
{
    public class ChatMessage : IChatMessage
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ExternalMessageId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ChatMessage()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}
