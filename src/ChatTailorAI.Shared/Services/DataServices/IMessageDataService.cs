using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;

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