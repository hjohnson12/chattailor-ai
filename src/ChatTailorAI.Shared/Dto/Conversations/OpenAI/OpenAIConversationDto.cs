using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Models.Conversations.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Conversations.OpenAI
{
    public class OpenAIConversationDto : ConversationDto
    {
        public OpenAIConversationDto() : base()
        {
            ConversationType = "OpenAI";
        }

        public OpenAIConversationDto(Conversation conversation) : base(conversation)
        {
            ConversationType = "OpenAI";
        }

        public static OpenAIConversationDto FromConversation(OpenAIConversation conversation)
        {
            return new OpenAIConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                Instructions = conversation.Instructions
            };
        }

        public override void UpdateEntity(Conversation conversation)
        {
            base.UpdateEntity(conversation);
        }
    }
}
