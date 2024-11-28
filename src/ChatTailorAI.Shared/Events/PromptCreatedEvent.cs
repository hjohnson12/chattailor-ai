using ChatTailorAI.Shared.Models.Prompts;

namespace ChatTailorAI.Shared.Events
{
    public class PromptCreatedEvent
    {
        public Prompt Prompt { get; set; }
    }
}