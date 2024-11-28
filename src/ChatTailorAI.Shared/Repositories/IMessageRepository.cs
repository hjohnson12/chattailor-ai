using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Services.Database.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<ChatMessage>> GetAllByConversationIdAsync(string conversationId);
        Task AddAsync(ChatMessage message);
        Task DeleteAllAsync();
        Task DeleteAsync(ChatMessage message);
        Task<ChatMessage> GetAsync(string id);
        Task UpdateAsync(ChatMessage message);
    }
}
