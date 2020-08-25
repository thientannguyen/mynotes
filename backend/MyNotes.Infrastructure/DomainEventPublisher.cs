using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.SharedKernel;
using MediatR;

namespace MyNotes.Infrastructure
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMediator _mediator;

        public DomainEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                var notificationType = typeof(NotificationDomainEvent<>).MakeGenericType(@event.GetType());
                var notification = Activator.CreateInstance(notificationType, @event) as INotification;
                await _mediator.Publish(notification);
            }
        }
    }
}
