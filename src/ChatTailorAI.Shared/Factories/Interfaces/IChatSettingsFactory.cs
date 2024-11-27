using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    public interface IChatSettingsFactory
    {
        ChatSettings CreateChatSettings(string chatServiceType);
        OpenAIChatSettings CreateOpenAIChatSettings();
    }
}
