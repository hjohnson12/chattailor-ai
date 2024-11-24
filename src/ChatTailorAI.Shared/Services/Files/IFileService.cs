using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;

namespace ChatTailorAI.Shared.Services.Files
{
    public interface IFileService
    {
        Task SaveToFileAsync(string filename, string data);
        Task<string> ReadFromFileAsync();
        Task UpdatePromptsFlieAsync(List<PromptDto> prompts);
        Task<List<PromptDto>> GetPromptsAsync();
        Task AppendPromptToFileAsync(PromptDto prompt);
        Task<List<PromptDto>> ReadPromptsFromFileAsync();
        Task UpdatePromptsAsync(List<PromptDto> prompts);
        Task DeletePromptsAsync();
        Task BackupPromptsAsync();
        Task<bool> CheckIfFileExists(string filename);
    }
}