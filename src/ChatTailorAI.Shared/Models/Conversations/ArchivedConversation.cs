using System;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations.OpenAI;

namespace ChatTailorAI.Shared.Models.Conversations
{
    public class ArchivedConversation
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

        public ArchivedConversation()
        {
            ArchivedAt = DateTime.UtcNow;
        }

        public ArchivedConversation(Conversation conversation)
        {
            Id = conversation.Id;
            Title = conversation.Title;
            Model = conversation.Model;
            CreatedAt = conversation.CreatedAt;
            ConversationType = conversation.ConversationType;
            Instructions = conversation.Instructions;
            DeletedAt = conversation.DeletedAt;
            ArchivedAt = DateTime.UtcNow;

            if (conversation is OpenAIAssistantConversation openAIAssistantConversation)
            {
                ThreadId = openAIAssistantConversation.ThreadId;
                AssistantType = openAIAssistantConversation.AssistantType;
            }
            if (conversation is AssistantConversation assistantConversation)
            {
                AssistantId = assistantConversation.AssistantId;
                AssistantType = assistantConversation.AssistantType;
            }
        }
    }
}