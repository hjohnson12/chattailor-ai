using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;

namespace ChatTailorAI.Shared.Services.DataServices
{
    public interface IPromptDataService
    {
        Task<IEnumerable<PromptDto>> GetAllAsync();
        Task AddPromptAsync(PromptDto prompt);
        Task DeleteAllAsync();
        Task DeleteAsync(PromptDto prompt);
        Task<PromptDto> GetAsync(string id);
        Task UpdateAsync(PromptDto prompt);
    }
}