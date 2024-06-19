using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.LMStudio;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Chat.LMStudio
{
    public interface ILMStudioChatService
    {
        void CancelStream();
        Task<List<string>> GetModels();
        Task<LMStudioChatResponseDto> GenerateChatResponseAsync(LMStudioChatRequest chatRequest);
    }
}
