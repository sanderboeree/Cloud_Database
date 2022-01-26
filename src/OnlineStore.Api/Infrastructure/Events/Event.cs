using OnlineStore.Api.Infrastructure.Events.Interfaces;
using MediatR;
using System;

namespace OnlineStore.Api.Infrastructure.Events
{
    public abstract class Event : INotification, IEvent
    {
        public DateTime Timestamp { get; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
