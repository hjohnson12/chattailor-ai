using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat.Google;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Chat.Anthropic
{
    public interface IAnthropicChatService : IBaseChatService
    {
        Task<AnthropicChatResponseDto> GenerateChatResponseAsync(AnthropicChatRequest chatRequest);
    }
}