using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.LMStudio
{
    public class LMStudioChatSettings : ChatSettings
    {
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }
        public bool Stream { get; set; }
    }
}
