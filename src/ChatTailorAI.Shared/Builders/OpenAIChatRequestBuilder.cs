using System.Collections.Generic;
using System.Linq;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;

namespace ChatTailorAI.Shared.Builders
{
    public class OpenAIChatRequestBuilder : IChatRequestBuilder
    {
        public object BuildChatRequest(string model, string instructions, IEnumerable<object> messages, object settings)
        {
            var openAIMessages = messages.Cast<OpenAIBaseChatMessageDto>();
            var openAISettings = (OpenAIChatSettings)settings;

            var openAIChatRequest = new OpenAIChatRequest
            {
                Model = model,
                Instructions = instructions,
                Messages = openAIMessages.ToList(),
                Settings = openAISettings
            };

            return openAIChatRequest;
        }
    }
}
