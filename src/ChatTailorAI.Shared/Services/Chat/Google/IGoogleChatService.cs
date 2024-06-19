using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Chat.Google
{
    public interface IGoogleChatService : IBaseChatService
    {
        Task<GoogleChatResponseDto> GenerateChatResponseAsync(GoogleChatRequest chatRequest);
    }
}
