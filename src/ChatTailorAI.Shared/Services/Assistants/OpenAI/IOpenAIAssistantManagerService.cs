using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Assistants.OpenAI
{
    public interface IOpenAIAssistantManagerService
    {
        Task<string> CreateAssistant(AssistantDto assistant);
        Task DeleteAssistant(string id);
        Task<AssistantDto> GetAssistant(string id);
        Task<List<AssistantDto>> GetAssistants();
        Task UpdateAssistant(AssistantDto assistant);
        Task<List<OpenAIThreadMessage>> SendMessage(string assistantId, string threadId, string message);
        Task<OpenAIThread> CreateThreadAsync();
    }
}
