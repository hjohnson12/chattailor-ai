using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IModelManagerService
    {
        IReadOnlyList<string> GetAllModels();
        Task RefreshDynamicModelsAsync();
    }
}