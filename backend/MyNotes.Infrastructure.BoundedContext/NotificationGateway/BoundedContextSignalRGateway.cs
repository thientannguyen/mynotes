using System;
using System.Threading.Tasks;
using MyNotes.Application.BoundedContext;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace MyNotes.Infrastructure.BoundedContext.NotificationGateway
{
    public class BoundedContextSignalRGateway : IBoundedContextNotification
    {
        private readonly string _notificationHubUrl;
        private readonly ILogger _logger;

        public BoundedContextSignalRGateway(string notificationHubUrl, ILogger logger)
        {
            _notificationHubUrl = notificationHubUrl;
            _logger = logger;
        }


        public async Task Notify(string entityId)
        {
            _logger.LogInformation("Notifying for " + entityId);
            var connection = await ConnectToHub();
            try
            {
                _logger.LogInformation("InvokeAsync 1 " + _notificationHubUrl);
                await connection.InvokeAsync("Notify", entityId);
                _logger.LogInformation("InvokeAsync 2 " + _notificationHubUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error upon notifying: " + ex.Message);
            }
            finally
            {
                await connection.StopAsync();
            }
        }

        private async Task<HubConnection> ConnectToHub()
        {
            _logger.LogInformation("ConnectToHub " + _notificationHubUrl);
            var connection = new HubConnectionBuilder()
                .WithUrl(_notificationHubUrl)
                .Build();

            await connection.StartAsync();
            _logger.LogInformation("ConnectToHub StartAsync " + _notificationHubUrl);
            return connection;
        }
    }
}