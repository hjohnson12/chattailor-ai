using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Models.Tools;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;

namespace ChatTailorAI.Services.Assistants.OpenAI
{
    public class OpenAIAssistantService : IOpenAIAssistantService
    {
        private static HttpClient _httpClient;
        private IAppSettingsService _appSettingsService;
        private IUserSettingsService _userSettingsService;
        private readonly IEventAggregator _eventAggregator;
        private Dictionary<string, string> _allowedTools;

        public OpenAIAssistantService(
            IAppSettingsService appSettingsService,
            IUserSettingsService userSettingsService,
            IEventAggregator eventAggregator,
            HttpClient client)
        {
            _appSettingsService = appSettingsService;
            _userSettingsService = userSettingsService;
            _eventAggregator = eventAggregator;

            var openAiApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != "" ?
                _appSettingsService.OpenAiApiKey :
                _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            _eventAggregator.ApiKeyChanged += OnApiKeyChanged;

            _allowedTools = new Dictionary<string, string>();
            _allowedTools.Add("Code Interpreter", "code_interpreter");

            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
            client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
            _httpClient = client;
        }

        private void OnApiKeyChanged(object sender, Shared.Events.EventArgs.ApiKeyChangedEventArgs e)
        {
            if (e.KeyType == Shared.Events.ApiKeyType.OpenAI)
            {
                UpdateApiKey();
            }
        }

        public void UpdateApiKey()
        {
            var newApiKey = _appSettingsService.OpenAiApiKey != null && _appSettingsService.OpenAiApiKey != ""
                ? _appSettingsService.OpenAiApiKey
                : _userSettingsService.Get<string>(UserSettings.OpenAiApiKey);

            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {newApiKey}");
        }


        public async Task<AssistantDto> CreateAssistantAsync(AssistantDto assistant)
        {
            //var assistantData = new
            //{
            //    model = "gpt-4-1106-preview", // TODO: Change later
            //    name,
            //    description,
            //    instructions,
            //    tools,
            //    file_ids = fileIds,
            //    metadata
            //};
            UpdateApiKey();

            var toolsExist = assistant.Tools != null && assistant.Tools.Count > 0;
            var assistantTools = toolsExist
                ? assistant.Tools
                    .Select(tool => _allowedTools.ContainsKey(tool.Type)
                        ? new Tool { Type = _allowedTools[tool.Type] }
                        : tool)
                    .ToArray()
                : new List<Tool>().ToArray();

            var assistantData = new
            {
                model = assistant.Model, // TODO: Change later
                name = assistant.Name,
                description = assistant.Description,
                instructions = assistant.Instructions,
                tools = assistantTools
            };

            var json = JsonConvert.SerializeObject(assistantData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("v1/assistants", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AssistantDto>(result);
        }


        public async Task<AssistantDto> RetrieveAssistantAsync(string assistantId)
        {
            var response = await _httpClient.GetAsync($"v1/assistants/{assistantId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Assistant must have been deleted somehow externally through OpenAI UI
                return null; 
            }

            response.EnsureSuccessStatusCode(); 

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AssistantDto>(result);
        }

        public async Task<AssistantDto> ModifyAssistantAsync(string assistantId, string model = null, string name = null, string description = null, string instructions = null, List<Tool> tools = null, List<string> fileIds = null, Dictionary<string, string> metadata = null)
        {
            // TODO: Add metadata key
            var assistantTools = (tools != null && tools.Count > 0) ? tools : new List<Tool>();
            var assistantFiles = (fileIds != null && fileIds.Count > 0) ? fileIds : new List<string>();
            var assistantData = new
            {
                model,
                name,
                description,
                instructions,
                tools = assistantTools,
                file_ids = assistantFiles,
            };

            var json = JsonConvert.SerializeObject(assistantData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"v1/assistants/{assistantId}", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AssistantDto>(result);
        }

        public async Task<bool> DeleteAssistantAsync(string assistantId)
        {
            var response = await _httpClient.DeleteAsync($"v1/assistants/{assistantId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AssistantDto>> ListAssistantsAsync(int limit = 20, string order = "desc", string after = null, string before = null)
        {
            var response = await _httpClient.GetAsync($"v1/assistants?limit={limit}&order={order}&after={after}&before={before}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var assistants = JsonConvert.DeserializeObject<List<AssistantDto>>(result);
            return assistants;
        }

        public async Task<OpenAIAssistantFile> CreateAssistantFileAsync(string assistantId, string fileId)
        {
            var fileData = new { file_id = fileId };
            var json = JsonConvert.SerializeObject(fileData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"v1/assistants/{assistantId}/files", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenAIAssistantFile>(result);
        }

        public async Task<OpenAIAssistantFile> RetrieveAssistantFileAsync(string assistantId, string fileId)
        {
            var response = await _httpClient.GetAsync($"v1/assistants/{assistantId}/files/{fileId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenAIAssistantFile>(result);
        }

        public async Task<bool> DeleteAssistantFileAsync(string assistantId, string fileId)
        {
            var response = await _httpClient.DeleteAsync($"v1/assistants/{assistantId}/files/{fileId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<OpenAIAssistantFile>> ListAssistantFilesAsync(string assistantId, int limit = 20, string order = "desc", string after = null, string before = null)
        {
            var response = await _httpClient.GetAsync($"v1/assistants/{assistantId}/files?limit={limit}&order={order}&after={after}&before={before}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var assistantFiles = JsonConvert.DeserializeObject<List<OpenAIAssistantFile>>(result);
            return assistantFiles;
        }
    }
}
