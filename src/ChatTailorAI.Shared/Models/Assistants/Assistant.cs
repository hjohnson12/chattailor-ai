using ChatTailorAI.Shared.Base;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChatTailorAI.Shared.Models.Assistants
{
    public enum AssistantType
    {
        OpenAI,
        Custom
    }

    public interface IAssistant
    {
        string Id { get; set; }
        string ExternalAssistantId { get; set; }
        string Name { get; set; }
        string Model { get; set; }
        string Description { get; set; }
        string CreatedAt { get; set; }
        string Instructions { get; set; }
        AssistantType AssistantType { get; set; }
        bool IsDeleted { get; set; }
    }
    public class Assistant : IAssistant
    {
        public string Id { get; set; }
        public string ExternalAssistantId { get; set; }
        public AssistantType AssistantType { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string Instructions { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Assistant()
        {
            IsDeleted = false;
        }

        // No OnPropertyChanged calls here; this is just for data access
    }
}
