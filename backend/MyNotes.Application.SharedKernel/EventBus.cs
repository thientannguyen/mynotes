namespace MyNotes.Application.SharedKernel
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Wsa.EventBus.Contracts;

    public class EventBus : IEventBus
    {
        private const string HandleMethodName = "Handle";
        private readonly IServiceCollection _serviceCollection;
        private readonly Lazy<IServiceProvider> _lazyServiceProvider;
        public EventBus(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            _lazyServiceProvider = new Lazy<IServiceProvider>(_serviceCollection.BuildServiceProvider);
        }

        private IServiceProvider ServiceProvider => _lazyServiceProvider.Value;

        public async Task Publish<T>(T @event) where T : IntegrationEvent
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var allHandlers = scope
                    .ServiceProvider
                    .GetServices(MakeGenericType(@event))
                    .ToList();

                foreach (var handler in allHandlers)
                {
                    var method = handler.GetType().GetMethod(HandleMethodName);
                    var handlerResultTask = method?.Invoke(handler, new object[] { @event }) as Task;
                    if (handlerResultTask != null) await handlerResultTask;
                }
            }
        }

        private static Type MakeGenericType<T>(T @event) where T : IntegrationEvent =>
            typeof(IIntegrationEventHandler<>).MakeGenericType(@event.GetType());

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _serviceCollection.AddTransient(typeof(IIntegrationEventHandler<T>), typeof(TH));
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException("Not needed for own in-process impl.");
        }
    }
}