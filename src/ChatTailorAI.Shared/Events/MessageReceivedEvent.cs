using ChatTailorAI.Shared.Dto.Chat;

namespace ChatTailorAI.Shared.Events
{
    public class MessageReceivedEvent
    {
        public ChatResponseDto Message { get; set; }
    }
}
