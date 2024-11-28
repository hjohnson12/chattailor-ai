using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Tools;
using System.Collections.Generic;

namespace ChatTailorAI.Shared.Dto
{
    public class AssistantDto
    {
        public string Id { get; set; }
        public string ExternalAssistantId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string Instructions { get; set; }
        public AssistantType AssistantType { get; set; }
        public List<Tool> Tools { get; set; }

        public AssistantDto() { }

        public AssistantDto(Assistant assistant)
        {
            Id = assistant.Id;
            ExternalAssistantId = assistant.ExternalAssistantId;
            Name = assistant.Name;
            Model = assistant.Model;
            Description = assistant.Description;
            CreatedAt = assistant.CreatedAt;
            Instructions = assistant.Instructions;
            AssistantType = assistant.AssistantType;
        }
    }
}
