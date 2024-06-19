using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Conversations
{
    public class AssistantConversationDto : ConversationDto
    {
        public AssistantType AssistantType { get; set; }
        public string AssistantId { get; set; }

        public AssistantConversationDto() : base()
        {
            ConversationType = "Assistant";
        }

        public AssistantConversationDto(Conversation conversation) : base(conversation)
        {
            if (conversation is AssistantConversation assistantConversation)
            {
                AssistantType = assistantConversation.AssistantType;
                AssistantId = assistantConversation.AssistantId;
            }
            else
            {
                throw new InvalidOperationException("Invalid conversation type for AssistantConversationDto.");
            }
        }

        public static AssistantConversationDto FromConversation(AssistantConversation conversation)
        {
            return new AssistantConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                AssistantType = conversation.AssistantType,
                AssistantId = conversation.AssistantId
            };
        }

        public override void UpdateEntity(Conversation conversation)
        {
            base.UpdateEntity(conversation);

            if (conversation is AssistantConversation assistantConversation)
            {
                assistantConversation.AssistantType = AssistantType;
                assistantConversation.AssistantId = AssistantId;
            }
            else
            {
                throw new InvalidOperationException("Invalid conversation type for AssistantConversationDto.");
            }
        }
    }
}
