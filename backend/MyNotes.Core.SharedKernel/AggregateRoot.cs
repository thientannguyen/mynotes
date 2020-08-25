using System.Diagnostics.CodeAnalysis;

namespace MyNotes.Core.SharedKernel
{
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    public abstract class AggregateRoot : Entity
    {
        // ReSharper disable once UnusedMember.Global Constructor is used
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(string id)
            : base(id)
        {
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public virtual IEnumerable<IDomainEvent> Events => _events;
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();

        protected void AddEvent(IDomainEvent @event) => _events.Add(@event);

        private bool _deleted;

        public virtual void Delete() => _deleted = true;


        public virtual bool IsDeleted() => _deleted;
    }
}
