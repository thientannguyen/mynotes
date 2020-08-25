using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MyNotes.Application.UserManagement.Notification;

namespace MyNotes.Infrastructure.UserManagement.NotificationGateway
{
    public class UserManagementNotificationSignalRGateway : IUserManagementNotification
    {
        private readonly string _notificationHubUrl;

        public UserManagementNotificationSignalRGateway(string notificationHubUrl)
        {
            _notificationHubUrl = notificationHubUrl;
        }

        public async Task Notify(string entityId)
        {
            var connection = await ConnectToHub();
            try
            {
                await connection.InvokeAsync("Notify", entityId);
            }
            finally
            {
                await connection.StopAsync();
            }
        }

        private async Task<HubConnection> ConnectToHub()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(_notificationHubUrl)
                .Build();

            await connection.StartAsync();
            return connection;
        }
    }
}
