using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat.Anthropic;

namespace ChatTailorAI.Shared.Services.Chat.Anthropic
{
    public interface IAnthropicChatService : IBaseChatService
    {
        Task<AnthropicChatResponseDto> GenerateChatResponseAsync(AnthropicChatRequest chatRequest);
    }
}