using System.Collections.Generic;
using System.Linq;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.Google;

namespace ChatTailorAI.Shared.Builders
{
    public class GoogleChatRequestBuilder : IChatRequestBuilder
    {
        public object BuildChatRequest(string model, string instructions, IEnumerable<object> messages, object settings)
        {
            var messageDtos = messages.Cast<GoogleBaseChatMessageDto>();
            var modelSettings = (GoogleChatSettings)settings;

            var googleChatRequest = new GoogleChatRequest
            {
                Model = model,
                Instructions = instructions,
                Messages = messageDtos.ToList(),
                Settings = modelSettings
            };

            return googleChatRequest;
        }
    }
}