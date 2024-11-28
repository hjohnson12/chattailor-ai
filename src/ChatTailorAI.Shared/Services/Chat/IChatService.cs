using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Services.Chat
{
    public interface IChatService<TSettings, TMessage, TResponse> : IBaseChatService
    {
        Task<TResponse> GenerateChatResponseAsync(ChatRequest<TSettings, TMessage> chatRequest);
    }
}