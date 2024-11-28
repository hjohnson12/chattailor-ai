using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.Google;

namespace ChatTailorAI.Shared.Services.Chat.Google
{
    public interface IGoogleChatService : IBaseChatService
    {
        Task<GoogleChatResponseDto> GenerateChatResponseAsync(GoogleChatRequest chatRequest);
    }
}