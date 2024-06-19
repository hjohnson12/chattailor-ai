using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Prompts
{
    public interface IPromptService
    {
        Task MigratePromptsAsync();
    }
}
