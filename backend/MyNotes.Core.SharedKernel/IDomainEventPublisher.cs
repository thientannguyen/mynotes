using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNotes.Core.SharedKernel
{
    public interface IDomainEventPublisher
    {
        Task Publish(IEnumerable<IDomainEvent> events);
    }
}
