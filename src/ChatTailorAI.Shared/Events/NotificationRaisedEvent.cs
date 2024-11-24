namespace ChatTailorAI.Shared.Events
{
    public class NotificationRaisedEvent
    {
        public string Message { get; }
        public int Duration { get; }

        public NotificationRaisedEvent(string message, int duration)
        {
            Message = message;
            Duration = duration;
        }
    }
}