using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Events
{
    /// <summary>
    /// Represents an event that is raised when an assistant is created.
    /// </summary>
    public class AssistantCreatedEvent
    {
        /// <summary>
        /// Gets or sets the created assistant.
        /// </summary>
        public AssistantDto Assistant { get; set; }
    }
}