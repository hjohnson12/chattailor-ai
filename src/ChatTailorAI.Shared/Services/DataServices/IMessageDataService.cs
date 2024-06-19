using ChatTailorAI.Shared.Dto.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.DataServices
{
    public interface IMessageDataService
    {
        Task<IEnumerable<ChatMessageDto>> GetMessagesAsync(string conversationId);
        Task SaveMessageAsync(ChatMessageDto chatMessageDto);
        Task DeleteMessageAsync(ChatMessageDto chatMessageDto);
        Task DeleteMessagesAsync();
        Task UpdateMessageAsync(ChatMessageDto chatMessageDto);

    }
}
