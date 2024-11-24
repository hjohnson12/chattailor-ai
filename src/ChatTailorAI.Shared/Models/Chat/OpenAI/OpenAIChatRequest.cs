using System.Collections.Generic;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;

namespace ChatTailorAI.Shared.Models.Chat.OpenAI
{
    public class OpenAIChatRequest : ChatRequest<OpenAIChatSettings, OpenAIBaseChatMessageDto>
    {
        public OpenAIChatRequest() { }

        public OpenAIChatRequest(List<OpenAIBaseChatMessageDto> messages, OpenAIChatSettings chatSettings)
        {
            Messages = messages;
            Settings = chatSettings;
        }
    }
}