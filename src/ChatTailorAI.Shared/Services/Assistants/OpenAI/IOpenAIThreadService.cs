using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;

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