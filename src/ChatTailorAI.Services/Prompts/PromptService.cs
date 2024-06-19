using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Services.Prompts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.Prompts
{
    public class PromptService : IPromptService
    {
        private readonly IPromptDataService _promptDataService;
        private readonly IFileService _fileService;

        public PromptService(
            IPromptDataService promptDataService,
            IFileService fileService)
        {
            _promptDataService = promptDataService;
            _fileService = fileService;
        }

        public async Task MigratePromptsAsync()
        {
            try
            {
                bool fileExists = await _fileService.CheckIfFileExists("prompts.json");
                if (fileExists == false)
                {
                    return;
                }

                var legacyPromptsFromFile = await _fileService.ReadPromptsFromFileAsync();
                await _fileService.BackupPromptsAsync();

                foreach (var legacyPrompt in legacyPromptsFromFile)
                {
                    await _promptDataService.AddPromptAsync(legacyPrompt);
                }

                await _fileService.DeletePromptsAsync();
            }
            catch (Exception ex)
            {
                // Worse case scenario, we can't migrate the prompts
                // We dont want to crash the app, so we just log the error
                Console.Error.WriteLine(ex);
            }
        }
    }
}
