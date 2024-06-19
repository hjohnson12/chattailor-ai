using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat
{
    public enum ModelType
    {
        OpenAIDalleImage,
        OpenAIGpt
    }

    // TODO: Save and load settings from db
    // (maybe) Specific model settings for the request will be saved in json format
    public class ChatSettings
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public ModelType ModelType { get; set; }
        public string ModelSettingsJson { get; set; } 
    }
}
