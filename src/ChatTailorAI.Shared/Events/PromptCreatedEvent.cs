using ChatTailorAI.Shared.Models.Prompts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Events
{
    public class PromptCreatedEvent
    {
        public Prompt Prompt { get; set; }
    }
}
