using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Assistants.OpenAI
{
    public interface IOpenAIThreadService
    {
        Task<OpenAIThread> CreateThreadAsync(string[] messages = null);
        Task RetrieveThreadAsync();
        Task ModifyThreadAsync();
        Task DeleteThreadAsync(string threadId);
    }
}
