using System;
using System.Collections.Generic;
using System.Text;

namespace MyNotes.Application.SharedKernel
{
    using System.Threading.Tasks;
    using Core.SharedKernel;
    using Wsa.EventBus.Contracts;

    public abstract class EventTranslator<TDomainEvent>
        : DomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private readonly IEventBus _eventBus;

        protected EventTranslator(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        protected override async Task Handle(TDomainEvent @event)
        {
            await _eventBus.Publish(Translate(@event));
        }

        public abstract IntegrationEvent Translate(TDomainEvent @event);
    }
}
