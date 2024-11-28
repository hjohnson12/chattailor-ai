using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;

namespace ChatTailorAI.Shared.Services.Chat.OpenAI
{
    public interface IOpenAIChatService : IBaseChatService
    {
        Task<OpenAIChatResponseDto> GenerateChatResponseAsync(OpenAIChatRequest chatRequest);
        void CancelStream();
    }
}