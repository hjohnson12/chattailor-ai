using System.Collections.Generic;
using System.Linq;
using ChatTailorAI.Shared.Dto.Chat.LMStudio;
using ChatTailorAI.Shared.Models.Chat.LMStudio;

namespace ChatTailorAI.Shared.Builders
{
    public class LMStudioChatRequestBuilder : IChatRequestBuilder
    {
        public object BuildChatRequest(string model, string instructions, IEnumerable<object> messages, object settings)
        {
            var LMStudioMessages = messages.Cast<LMStudioBaseChatMessageDto>();
            var LMStudioSettings = (LMStudioChatSettings)settings;

            var LMStudioChatRequest = new LMStudioChatRequest
            {
                Model = model,
                Instructions = instructions,
                Messages = LMStudioMessages.ToList(),
                Settings = LMStudioSettings
            };

            return LMStudioChatRequest;
        }
    }
}
