using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Chat.OpenAI
{
    public interface IOpenAIChatService : IBaseChatService
    {
        void CancelStream();
        Task<OpenAIChatResponseDto> GenerateChatResponseAsync(OpenAIChatRequest chatRequest);
    }
}
