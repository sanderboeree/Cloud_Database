using OnlineStore.Api.Infrastructure.Events.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(IEvent evt, CancellationToken cancellationToken = default)
        {
            if (evt == null)
            {
                throw new ArgumentNullException(nameof(evt));
            }

            await _mediator.Publish(evt, cancellationToken);
        }
    }
}
