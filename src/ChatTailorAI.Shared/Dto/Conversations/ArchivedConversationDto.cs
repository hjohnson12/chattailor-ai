using System;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;

namespace ChatTailorAI.Shared.Dto.Conversations
{
    /// <summary>
    /// A DTO for an archived conversation that is used to store a copy of the conversation
    /// </summary>
    public class ArchivedConversationDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ConversationType { get; set; }
        public string Instructions { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime ArchivedAt { get; set; }

        // Properties from AssistantConversation types
        public AssistantType? AssistantType { get; set; }
        public string AssistantId { get; set; }
        public string ThreadId { get; set; }
        
        public ArchivedConversationDto() { }

        public static ArchivedConversationDto FromEntity(ArchivedConversation entity)
        {
            return new ArchivedConversationDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Model = entity.Model,
                CreatedAt = entity.CreatedAt,
                ConversationType = entity.ConversationType,
                Instructions = entity.Instructions,
                DeletedAt = entity.DeletedAt,
                ArchivedAt = DateTime.UtcNow, 
                // Properties from AssistantConversation types
                AssistantType = entity.AssistantType,
                AssistantId = entity.AssistantId, 
                ThreadId = entity.ThreadId,
            };
        }

        public void UpdateEntity(ArchivedConversation entity)
        {
            entity.Id = Id;
            entity.Title = Title;
            entity.Model = Model;
            entity.CreatedAt = CreatedAt;
            entity.ConversationType = ConversationType;
            entity.Instructions = Instructions;
            entity.DeletedAt = DeletedAt;
            entity.AssistantType = AssistantType;
            entity.AssistantId = AssistantId;
            entity.ThreadId = ThreadId;
        }
    }
}