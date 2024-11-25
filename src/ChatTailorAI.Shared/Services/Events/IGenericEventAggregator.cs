using System;

namespace ChatTailorAI.Shared.Services.Events
{
    /// <summary>
    /// An interface for an event aggregator that can be used to publish and subscribe to events.
    /// </summary>
    public interface IGenericEventAggregator
    {
        /// <summary>
        /// Subscribes to an event of type TEvent.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="handler"></param>
        void Subscribe<TEvent>(Action<TEvent> handler);

        /// <summary>
        /// Unsubscribes from an event of type TEvent.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="handler"></param>
        void Unsubscribe<TEvent>(Action<TEvent> handler);

        /// <summary>
        /// Publishes an event of type TEvent.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventToPublish"></param>
        void Publish<TEvent>(TEvent eventToPublish);
    }
}