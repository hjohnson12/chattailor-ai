using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Models.Conversations.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Conversations.OpenAI
{
    public class OpenAIAssistantConversationDto : AssistantConversationDto
    {
        public string ThreadId { get; set; }

        public OpenAIAssistantConversationDto() : base()
        {
            // AssistantId will be provided from the OpenAI API
            ConversationType = "OpenAI Assistant";
        }

        public OpenAIAssistantConversationDto(Conversation conversation) : base(conversation)
        {
            if (conversation is OpenAIAssistantConversation openAIAssistantConversation)
            {
                ThreadId = openAIAssistantConversation.ThreadId;
            }
            else
            {
                throw new InvalidOperationException("Invalid conversation type for OpenAIAssistantConversationDto.");
            }
        }

        public static OpenAIAssistantConversationDto FromConversation(OpenAIAssistantConversation conversation)
        {
            return new OpenAIAssistantConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                AssistantType = conversation.AssistantType,
                AssistantId = conversation.AssistantId,
                ThreadId = conversation.ThreadId
            };
        }

        public override void UpdateEntity(Conversation conversation)
        {
            base.UpdateEntity(conversation);

            if (conversation is OpenAIAssistantConversation openAIAssistantConversation)
            {
                openAIAssistantConversation.AssistantType = AssistantType;
                openAIAssistantConversation.AssistantId = AssistantId;
                openAIAssistantConversation.ThreadId = ThreadId;
            }
            else
            {
                throw new InvalidOperationException("Invalid conversation type for OpenAIAssistantConversationDto.");
            }
        }
    }
}
