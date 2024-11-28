using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Prompts;

namespace ChatTailorAI.Shared.Repositories
{
    public interface IPromptRepository
    {
        Task<List<Prompt>> GetAllAsync();
        Task AddPromptAsync(Prompt prompt);
        Task DeleteAllAsync();
        Task DeleteAsync(Prompt prompt);
        Task<Prompt> GetAsync(string id);
        Task UpdateAsync(Prompt prompt);
    }
}
