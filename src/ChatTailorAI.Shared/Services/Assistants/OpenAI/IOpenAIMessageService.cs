using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;

namespace ChatTailorAI.Shared.Services.Assistants.OpenAI
{
    public interface IOpenAIMessageService
    {
        Task<OpenAIThreadMessage> CreateMessageAsync(string threadId, string message);
        Task RetrieveMessageAsync();
        Task ModifyMessageAsync();
        Task<List<OpenAIThreadMessage>> ListMessagesAsync(string threadId);
        Task RetrieveMessageFileAsync();
        Task ListMessageFilesAsync();
    }
}