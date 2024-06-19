using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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