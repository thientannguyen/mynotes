using System.Threading.Tasks;

namespace MyNotes.Application.BoundedContext
{
    public interface IBoundedContextNotification
    {
        Task Notify(string entityId);
    }
}