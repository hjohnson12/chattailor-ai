using ChatTailorAI.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
