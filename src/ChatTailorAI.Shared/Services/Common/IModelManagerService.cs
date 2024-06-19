using ChatTailorAI.Shared.Resources;
using ChatTailorAI.Shared.Services.Chat.LMStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IModelManagerService
    {
        IReadOnlyList<string> GetAllModels();
        Task RefreshDynamicModelsAsync();
    }
}