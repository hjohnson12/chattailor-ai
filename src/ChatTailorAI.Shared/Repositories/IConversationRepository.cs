using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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