using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using ChatTailorAI.Shared.Models.Tools;

namespace ChatTailorAI.Shared.Services.Assistants.OpenAI
{
    public interface IOpenAIAssistantService
    {
        // TODO: Hook up with OpenAIAssistant cause its missing props on Assistant
        Task<AssistantDto> CreateAssistantAsync(AssistantDto assistant);
        Task<AssistantDto> RetrieveAssistantAsync(string assistantId);
        Task<AssistantDto> ModifyAssistantAsync(string assistantId, string model = null, string name = null, string description = null, string instructions = null, List<Tool> tools = null, List<string> fileIds = null, Dictionary<string, string> metadata = null);
        Task<bool> DeleteAssistantAsync(string assistantId);
        Task<List<AssistantDto>> ListAssistantsAsync(int limit = 20, string order = "desc", string after = null, string before = null);
        Task<OpenAIAssistantFile> CreateAssistantFileAsync(string assistantId, string fileId);
        Task<OpenAIAssistantFile> RetrieveAssistantFileAsync(string assistantId, string fileId);
        Task<bool> DeleteAssistantFileAsync(string assistantId, string fileId);
        Task<List<OpenAIAssistantFile>> ListAssistantFilesAsync(string assistantId, int limit = 20, string order = "desc", string after = null, string before = null);
    }
}