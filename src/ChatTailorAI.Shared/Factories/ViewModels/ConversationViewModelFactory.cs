using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Dto.Conversations.OpenAI;
using ChatTailorAI.Shared.ViewModels;

namespace ChatTailorAI.Shared.Factories.ViewModels
{
    public class ConversationViewModelFactory
    {
        public static ConversationViewModel CreateViewModel(ConversationDto dto)
        {
            switch (dto)
            {
                case OpenAIConversationDto _:
                    return new OpenAIConversationViewModel { };
                case OpenAIAssistantConversationDto _:
                    return new OpenAIAssistantConversationViewModel { };
                case AssistantConversationDto _:
                    return new AssistantConversationViewModel { };
                default:
                    return new ConversationViewModel { };
            }
        }
    }
}