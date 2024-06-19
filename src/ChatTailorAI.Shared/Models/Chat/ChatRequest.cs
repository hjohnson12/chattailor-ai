using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat
{
    public class ChatRequest<TSettings, TMessage>
    {
        public string Model { get; set; }
        public string Instructions { get; set; }
        public List<TMessage> Messages { get; set; }
        public TSettings Settings { get; set; }
    }
}