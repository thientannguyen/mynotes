namespace MyNotes.Infrastructure
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using Wsa.EventBus.Contracts;

    public interface IBoundedContextSetup
    {
        void Register(IServiceCollection services);
        void AddAuthorization(AuthorizationOptions options, string issuer);
        IEnumerable<Assembly> GetMediatRAssemblies();
        void SubscribeToEventBus(IEventBus eventBus);
    }
}