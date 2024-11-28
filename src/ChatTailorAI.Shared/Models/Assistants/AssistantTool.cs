using ChatTailorAI.Shared.Models.Tools;

namespace ChatTailorAI.Shared.Models.Assistants
{
    // Using as a junction table for many-to-many relationship between Assistant and Tool
    public class AssistantTool
    {
        // Foreign key for Assistant
        public int AssistantId { get; set; }
        public Assistant Assistant { get; set; }

        // Foreign key for Tool
        public int ToolId { get; set; }
        public Tool Tool { get; set; }
    }
}