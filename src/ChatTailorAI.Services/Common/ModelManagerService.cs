using ChatTailorAI.Shared.Resources;
using ChatTailorAI.Shared.Services.Chat.LMStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Services.Common
{
    public class ModelManagerService : IModelManagerService
    {
        private readonly ILMStudioChatService _lmStudioChatService;
        private readonly List<string> _staticModels;
        private List<string> _dynamicModels;
        private readonly object _lock = new object();

        public ModelManagerService(ILMStudioChatService lmStudioChatService)
        {
            _lmStudioChatService = lmStudioChatService;
            _staticModels = ModelConstants.DefaultChatModels.ToList();
            _dynamicModels = new List<string>();
        }

        public IReadOnlyList<string> GetAllModels()
        {
            lock (_lock)
            {
                return _staticModels.Concat(_dynamicModels).ToList();
            }
        }

        public async Task RefreshDynamicModelsAsync()
        {
            var apiModels = await FetchModelsFromApi();
            lock (_lock)
            {
                _dynamicModels = apiModels;
            }
        }

        private async Task<List<string>> FetchModelsFromApi()
        {
            try
            {
                var response = await _lmStudioChatService.GetModels();
                return response;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return new List<string>();
            }
        }
    }
}
