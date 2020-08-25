using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MyNotes.API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Notify(string entityId)
        {
            await Clients.Group(entityId).SendCoreAsync("NotifyChange", new object[] { entityId });
        }

        public async Task Register(string entityId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, entityId);
        }

        public async Task Unregister(string entityId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, entityId);
        }
    }
}
