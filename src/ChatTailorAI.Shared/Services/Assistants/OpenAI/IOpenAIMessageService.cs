using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
