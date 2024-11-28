using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Dto;
using Windows.Storage.Pickers;

namespace ChatTailorAI.Services.Uwp.FileManagement
{
    public class FileService : IFileService
    {
        public async Task SaveToFileAsync(string filename, string data)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

            savePicker.SuggestedFileName = filename;

            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                await FileIO.WriteTextAsync(file, data);

                await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        public async Task<string> ReadFromFileAsync()
        {
            var openPicker = new FileOpenPicker();

            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                string data = await FileIO.ReadTextAsync(file);
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdatePromptsFlieAsync(List<PromptDto> prompts)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder
                .CreateFileAsync("prompts.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(prompts));
        }

        public async Task<List<PromptDto>> GetPromptsAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                var readFile = await localFolder.GetFileAsync("prompts.json");
                return JsonConvert.DeserializeObject<List<PromptDto>>(await FileIO.ReadTextAsync(readFile));
            }
            catch (FileNotFoundException)
            {
                return new List<PromptDto>();
            }
        }

        public async Task AppendPromptToFileAsync(PromptDto prompt)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync("prompts.json", CreationCollisionOption.OpenIfExists);

            var promptJson = JsonConvert.SerializeObject(prompt);

            // Append the new prompt to the file with a newline at the end
            await FileIO.AppendTextAsync(file, promptJson + "\n");
        }

        public async Task<bool> CheckIfFileExists(string filename)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                var readFile = await localFolder.GetFileAsync(filename);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public async Task<List<PromptDto>> ReadPromptsFromFileAsync()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync("prompts.json");

                var lines = await FileIO.ReadLinesAsync(file);

                var prompts = lines.Select(line => JsonConvert.DeserializeObject<PromptDto>(line)).ToList();

                return prompts;
            }
            catch (FileNotFoundException)
            {
                return new List<PromptDto>();
            }
        }

        public async Task DeletePromptAsync(string promptId)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync("prompts.json");

            var lines = await FileIO.ReadLinesAsync(file);

            var newLines = new List<string>();

            foreach (var line in lines)
            {
                var prompt = JsonConvert.DeserializeObject<PromptDto>(line);
                if (Convert.ToInt32(prompt.Id) != Convert.ToInt32(promptId))
                {
                    newLines.Add(line);
                }
            }

            await FileIO.WriteLinesAsync(file, newLines);
        }

        public async Task UpdatePromptsAsync(List<PromptDto> prompts)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync("prompts.json");

            List<string> newLines = prompts
                .Select(prompt => JsonConvert.SerializeObject(prompt))
                .ToList();

            await FileIO.WriteLinesAsync(file, newLines);
        }

        public async Task BackupPromptsAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.GetFileAsync("prompts.json");
                if (file != null)
                {
                    var backupFolder = await localFolder.CreateFolderAsync("Backups", CreationCollisionOption.OpenIfExists);
                    await file.CopyAsync(backupFolder, "prompts.json", NameCollisionOption.GenerateUniqueName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to backup prompts: {ex.Message}");
            }
        }

        public async Task DeletePromptsAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.GetFileAsync("prompts.json");

                if (file != null)
                {
                    await file.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete prompts: {ex.Message}");
            }
        }
    }
}