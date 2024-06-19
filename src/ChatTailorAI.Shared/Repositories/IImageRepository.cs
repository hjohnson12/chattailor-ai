using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
