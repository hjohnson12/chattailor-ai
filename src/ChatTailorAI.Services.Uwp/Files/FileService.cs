using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using ChatTailorAI.Shared.Models.Prompts;
using System.Diagnostics;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Dto;

namespace ChatTailorAI.Services.Uwp.FileManagement
{
    public class FileService : IFileService
    {
        public async Task SaveToFileAsync(string filename, string data)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };

            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

            savePicker.SuggestedFileName = filename;

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);

                await Windows.Storage.FileIO.WriteTextAsync(file, data);

                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        public async Task<string> ReadFromFileAsync()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                string data = await Windows.Storage.FileIO.ReadTextAsync(file);
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdatePromptsFlieAsync(List<PromptDto> prompts)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder
                .CreateFileAsync("prompts.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(prompts));
        }

        public async Task<List<PromptDto>> GetPromptsAsync()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                var readFile = await localFolder.GetFileAsync("prompts.json");
                return JsonConvert.DeserializeObject<List<PromptDto>>(await Windows.Storage.FileIO.ReadTextAsync(readFile));
            }
            catch (FileNotFoundException)
            {
                return new List<PromptDto>();
            }
        }

        public async Task AppendPromptToFileAsync(PromptDto prompt)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync("prompts.json", Windows.Storage.CreationCollisionOption.OpenIfExists);

            var promptJson = JsonConvert.SerializeObject(prompt);

            // Append the new prompt to the file with a newline at the end
            await Windows.Storage.FileIO.AppendTextAsync(file, promptJson + "\n");
        }

        public async Task<bool> CheckIfFileExists(string filename)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
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
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync("prompts.json");

                var lines = await Windows.Storage.FileIO.ReadLinesAsync(file);

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
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync("prompts.json");

            var lines = await Windows.Storage.FileIO.ReadLinesAsync(file);

            var newLines = new List<string>();

            foreach (var line in lines)
            {
                var prompt = JsonConvert.DeserializeObject<PromptDto>(line);
                if (Convert.ToInt32(prompt.Id) != Convert.ToInt32(promptId))
                {
                    newLines.Add(line);
                }
            }

            await Windows.Storage.FileIO.WriteLinesAsync(file, newLines);
        }

        public async Task UpdatePromptsAsync(List<PromptDto> prompts)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync("prompts.json");

            List<string> newLines = prompts
                .Select(prompt => JsonConvert.SerializeObject(prompt))
                .ToList();

            await Windows.Storage.FileIO.WriteLinesAsync(file, newLines);
        }

        public async Task BackupPromptsAsync()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.GetFileAsync("prompts.json");
                if (file != null)
                {
                    var backupFolder = await localFolder.CreateFolderAsync("Backups", Windows.Storage.CreationCollisionOption.OpenIfExists);
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
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

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