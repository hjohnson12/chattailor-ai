using System;
using System.Collections.Generic;
using System.Text;

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
