using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Dto.Conversations.OpenAI;
using ChatTailorAI.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.ViewModels
{
    public class ConversationViewModelFactory
    {
        public static ConversationViewModel CreateViewModel(ConversationDto dto)
        {
            switch (dto)
            {
                case OpenAIConversationDto openAIDto:
                    return new OpenAIConversationViewModel { };
                case OpenAIAssistantConversationDto openAIAssistantDto:
                    return new OpenAIAssistantConversationViewModel { };
                case AssistantConversationDto assistantDto:
                    return new AssistantConversationViewModel { };
                default:
                    return new ConversationViewModel { };
            }
        }
    }
}