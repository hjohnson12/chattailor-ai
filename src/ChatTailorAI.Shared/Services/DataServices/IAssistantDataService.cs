using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;

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