using System.Threading.Tasks;
using MyNotes.Application.UserManagement.Notification;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.UserManagement.Events;

namespace MyNotes.Application.UserManagement.EventTranslations
{
    public class UserCreatedTranslator : DomainEventHandler<UserCreated>
    {
        private readonly IUserManagementNotification _notification;

        public UserCreatedTranslator(IUserManagementNotification notification)
        {
            _notification = notification;
        }

        protected override async Task Handle(UserCreated @event)
        {
            await _notification.Notify(@event.Id);
        }
    }
}