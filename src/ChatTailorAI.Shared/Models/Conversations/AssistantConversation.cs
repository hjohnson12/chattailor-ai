﻿using ChatTailorAI.Shared.Models.Assistants;

namespace ChatTailorAI.Shared.Models.Conversations
{
    public class AssistantConversation : Conversation
    {
        public AssistantType AssistantType { get; set; }
        public string AssistantId { get; set; }

        // Navigation Properties
        public virtual Assistant Assistant { get; set; }


        public AssistantConversation() : base()
        {
            ConversationType = "Assistant";
        }
    }

}