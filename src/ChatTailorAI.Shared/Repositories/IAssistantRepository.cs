using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Assistants;

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