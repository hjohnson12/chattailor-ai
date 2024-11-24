using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using ChatTailorAI.Shared.Models.Chat.LMStudio;

namespace ChatTailorAI.Shared.Services.Chat.LMStudio
{
    public interface ILMStudioChatService
    {
        void CancelStream();
        Task<List<string>> GetModels();
        Task<LMStudioChatResponseDto> GenerateChatResponseAsync(LMStudioChatRequest chatRequest);
    }
}