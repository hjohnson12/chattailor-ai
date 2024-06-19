using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Assistants.OpenAI
{
    public interface IOpenAIRunService
    {
        // TODO: Add types
        Task CreateRunAsync();
        Task<OpenAIThreadRun> CreateRunAsync(string assistantId, string threadId);
        Task<OpenAIThreadRun> RetrieveRunAsync(string runId, string threadId);
        Task ModifyRunAsync();
        Task ListRunsAsync();
        Task SubmitToolOutputsToRunAsync();
        Task CancelRun();
        Task CreateThreadAndRunAsync();
        Task RetrieveRunStepAsync();
        Task ListRunStepsAsync();
    }

}
