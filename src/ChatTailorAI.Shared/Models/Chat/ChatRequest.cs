using System.Collections.Generic;

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