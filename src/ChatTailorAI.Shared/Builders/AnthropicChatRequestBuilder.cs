using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat.Anthropic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatTailorAI.Shared.Builders
{
    public class AnthropicChatRequestBuilder : IChatRequestBuilder
    {
        public object BuildChatRequest(string model, string instructions, IEnumerable<object> messages, object settings)
        {
            var openAIMessages = messages.Cast<AnthropicBaseChatMessageDto>();
            var openAISettings = (AnthropicChatSettings)settings;

            var openAIChatRequest = new AnthropicChatRequest
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