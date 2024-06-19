using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.DataServices
{
    public interface IAssistantDataService
    {
        Task SaveAssistantAsync(AssistantDto assistant);
        Task UpdateAssistantAsync(AssistantDto assistant);
        Task<IEnumerable<AssistantDto>> GetAssistantsAsync();
        Task DeleteAssistantAsync(AssistantDto assistant);
        Task DeleteAssistantsAsync();
        Task<AssistantDto> GetAssistantAsync(AssistantDto assistant);
        Task<AssistantDto> GetAssistantByIdAsync(string id);
        Task<Dictionary<string,string>> GetAssistantNamesByIdsAsync(IEnumerable<string> ids);
    }
}