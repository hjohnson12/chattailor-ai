using ChatTailorAI.Shared.Services.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Services.Events
{
    public class GenericEventAggregator : IGenericEventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> _eventHandlers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (!_eventHandlers.TryGetValue(typeof(TEvent), out var handlers))
            {
                handlers = new List<Delegate>();
                _eventHandlers[typeof(TEvent)] = handlers;
            }

            handlers.Add(handler);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            if (_eventHandlers.TryGetValue(typeof(TEvent), out var handlers))
            {
                handlers.Remove(handler);
            }
        }

        public void Publish<TEvent>(TEvent eventToPublish)
        {
            if (_eventHandlers.TryGetValue(typeof(TEvent), out var handlers))
            {
                foreach (var handler in handlers)
                {
                    (handler as Action<TEvent>)?.Invoke(eventToPublish);
                }
            }
        }
    }
}
