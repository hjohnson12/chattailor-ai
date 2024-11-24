using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Chat;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    /// <summary>
    /// Interface for creating ChatServices
    /// </summary>
    public interface IChatServiceFactory
    {
        IChatService<TSettings, TMessage, TResponse> Create<TSettings, TMessage, TResponse>(string key)
            where TSettings : ChatSettings
            where TMessage : ChatMessageDto
            where TResponse : ChatMessageDto;
    }
}
