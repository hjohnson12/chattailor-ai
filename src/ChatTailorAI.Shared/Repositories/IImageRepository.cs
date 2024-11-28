using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Repositories
{
    public interface IImageRepository
    {
        Task AddAsync(ChatImage image);
        Task UpdateAsync(ChatImage image);
        Task DeleteAsync(ChatImage image);
        Task<ChatImage> GetAsync(int id);
        Task<IEnumerable<ChatImage>> GetAllAsync();
        Task<IEnumerable<ChatImage>> GetAllByMessageId(string messageId);
    }
}
