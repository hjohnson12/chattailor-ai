using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Services.Events;

namespace ChatTailorAI.Services.Uwp.Notification
{
    /// <summary>
    /// Class for displaying a message on the UI using InAppNotification from Microsoft Toolkit
    /// </summary>
    public class AppNotificationService : IAppNotificationService
    {
        private readonly IEventAggregator _eventAggregator;

        public AppNotificationService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Display a message on-screen to notify the user of something
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="duration">Duration in milliseconds</param>
        public void Display(string message, int duration = 5000)
        {
            _eventAggregator.PublishNotificationRaised(new NotificationRaisedEvent(message, duration));
        }
    }
}