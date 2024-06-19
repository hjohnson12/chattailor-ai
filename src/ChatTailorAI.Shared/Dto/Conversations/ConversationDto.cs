using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Conversations
{
    public interface IConversationDto<T> where T : Conversation
    {
        void UpdateEntity(T entity);
    }

    public class ConversationDto : IConversationDto<Conversation>
    {
        public ConversationDto()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            ConversationType = "Standard";
        }

        public ConversationDto(Conversation conversation)
        {
            Id = conversation.Id;
            Title = conversation.Title;
            Model = conversation.Model;
            CreatedAt = conversation.CreatedAt;
            ConversationType = conversation.ConversationType;
            Instructions = conversation.Instructions;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ConversationType { get; set; }
        public string Instructions { get; set; }

        public static ConversationDto FromConversation(Conversation conversation)
        {
            return new ConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                Instructions = conversation.Instructions

            };
        }

        public virtual void UpdateEntity(Conversation conversation)
        {
            conversation.Id = Id;
            conversation.Title = Title;
            conversation.Model = Model;
            conversation.CreatedAt = CreatedAt;
            conversation.ConversationType = ConversationType;
            conversation.Instructions = Instructions;
        }
    }
}