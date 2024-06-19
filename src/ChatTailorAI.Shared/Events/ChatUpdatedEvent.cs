using ChatTailorAI.Shared.Dto.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Events
{
    public class ChatUpdatedEvent
    {
        public ConversationDto Conversation { get; set; }
    }
}
