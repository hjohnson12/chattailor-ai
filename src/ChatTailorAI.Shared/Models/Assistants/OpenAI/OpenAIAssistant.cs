using ChatTailorAI.Shared.Models.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Assistants.OpenAI
{
    public class OpenAIAssistant : Assistant
    {
        public List<Tool> Tools { get; set; }
        public List<string> FileIds { get; set; }
        public Dictionary<string, string> Metadata { get; set; }

        public OpenAIAssistant()
        {
            IsDeleted = false;
            Tools = new List<Tool>();
        }
    }
}