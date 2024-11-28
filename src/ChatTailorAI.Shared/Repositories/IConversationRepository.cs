using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Conversations;

namespace ChatTailorAI.Shared.Services.Database.Repositories
{
    public interface IConversationRepository
    {
        Task AddConversationAsync(Conversation conversation);
        Task<List<Conversation>> GetAllAsync();
        Task<Conversation> GetConversationAsync(string id);
        Task DeleteAsync(string id);
        Task<Conversation> GetAsync(string id);
        Task UpdateAsync(Conversation conversation);
    }
}