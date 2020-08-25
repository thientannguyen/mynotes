using MyNotes.Core.SharedKernel;
using MediatR;

namespace MyNotes.Application.SharedKernel
{
    public class NotificationDomainEvent<T> : INotification where T : IDomainEvent
    {
        public readonly T Event;

        public NotificationDomainEvent(T @event)
        {
            Event = @event;
        }

        public static implicit operator NotificationDomainEvent<T>(T domainEvent) =>
            new NotificationDomainEvent<T>(domainEvent);
    }
}
