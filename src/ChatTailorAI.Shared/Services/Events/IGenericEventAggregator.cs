using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Services.Events
{
    public interface IGenericEventAggregator
    {
        void Subscribe<TEvent>(Action<TEvent> handler);
        void Unsubscribe<TEvent>(Action<TEvent> handler);
        void Publish<TEvent>(TEvent eventToPublish);
    }
}
