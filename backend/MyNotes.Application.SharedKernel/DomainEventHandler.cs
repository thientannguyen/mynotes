namespace MyNotes.Application.SharedKernel
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.SharedKernel;
    using MediatR;

    public abstract class DomainEventHandler<TDomainEvent>
        : INotificationHandler<NotificationDomainEvent<TDomainEvent>> where TDomainEvent : IDomainEvent
    {
        public virtual Task Handle(NotificationDomainEvent<TDomainEvent> notification, CancellationToken cancellationToken)
        {
            return Handle(notification.Event);
        }

        protected abstract Task Handle(TDomainEvent @event);
    }
}
