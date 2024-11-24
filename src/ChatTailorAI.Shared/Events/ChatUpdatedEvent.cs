using ChatTailorAI.Shared.Dto.Conversations;

namespace ChatTailorAI.Shared.Events
{
    public class ChatUpdatedEvent
    {
        public ConversationDto Conversation { get; set; }
    }
}