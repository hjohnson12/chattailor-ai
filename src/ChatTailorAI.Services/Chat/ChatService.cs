using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.Chat
{
    public class ChatService
    {
        private readonly IOpenAIChatService _openAIGptChatService;

        public ChatService(
            IOpenAIChatService openAIGptChatService) 
        {
            _openAIGptChatService = openAIGptChatService;
        }

        //public async Task<ChatResponseDto> GenerateChatResponseAsync(ChatRequestDto<ChatSettings, ChatMessage> chatRequest)
        //{
        //    var response = await _openAIGptChatService.GenerateChatResponseAsync(chatRequest);
        //    return response;
        //}
    }
}
