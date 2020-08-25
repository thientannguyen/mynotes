using System.Threading.Tasks;

namespace MyNotes.Application.UserManagement.Notification
{
    public interface IUserManagementNotification
    {
        Task Notify(string entityId);
    }
}
