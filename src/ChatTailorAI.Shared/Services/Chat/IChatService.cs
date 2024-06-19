using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Chat
{
    public interface IChatService<TSettings, TMessage, TResponse> : IBaseChatService
    {
        Task<TResponse> GenerateChatResponseAsync(ChatRequest<TSettings, TMessage> chatRequest);
    }

    //public interface IChatService
    //{
    //    Task<ChatResponseDto> GenerateChatResponseAsync(ChatRequestDto<ChatSettings, ChatMessage> chatRequest);
    //}
}