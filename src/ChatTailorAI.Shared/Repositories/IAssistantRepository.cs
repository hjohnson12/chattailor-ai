using ChatTailorAI.Shared.Models.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Repositories
{
    public interface IAssistantRepository
    {
        Task<List<Assistant>> GetAllAsync();
        Task AddAssistantAsync(Assistant assistant);
        Task DeleteAllAsync();
        Task DeleteAsync(Assistant assistant);
        Task<Assistant> GetAsync(string id);
        Task UpdateAsync(Assistant assistant);
        Task<List<Assistant>> GetByIdsAsync(IEnumerable<string> ids);
    }
}
