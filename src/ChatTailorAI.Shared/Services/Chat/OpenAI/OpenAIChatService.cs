using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Services.Chat.OpenAI
{
    public class OpenAIChatService : IChatService<OpenAIChatSettings, OpenAIChatResponseDto, OpenAIChatResponseDto>
    {
        public Task<OpenAIChatResponseDto> GenerateChatResponseAsync(ChatRequest<OpenAIChatSettings, OpenAIChatResponseDto> chatRequest)
        {
            var settings = JsonConvert.DeserializeObject<OpenAIChatSettings>(chatRequest.Settings.ModelSettingsJson);
            throw new Exception("Not implemented");
        }

        public bool ValidateApiKey()
        {
            throw new NotImplementedException();
        }

        //public async Task<OpenAIChatMessageDto> GenerateChatResponseAsync(ChatRequest chatRequest)
        //{
        //    // Implementation for generating chat response using OpenAI-specific settings
        //    // No need for casting as chatRequest.ChatSettings is already of type OpenAIChatSettings
        //}

        //public async Task<OpenAIChatMessageDto> GenerateChatResponseAsync(ChatRequest request)
        //{
        //    // You would deserialize the ModelSettingsJson here into OpenAIChatSettings
        //    var settings = JsonConvert.DeserializeObject<OpenAIChatSettings>(request.ChatSettings.ModelSettingsJson);

        //    // Now you can use 'settings' which is of type OpenAIChatSettings
        //    // Implementation for generating chat response using OpenAI-specific settings
        //}
    }
}
