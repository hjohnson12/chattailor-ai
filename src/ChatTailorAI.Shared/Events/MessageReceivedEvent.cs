using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Events
{
    public class MessageReceivedEvent
    {
        public ChatResponseDto Message { get; set; }
    }
}
