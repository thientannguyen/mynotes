using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MyNotes.Application.UserManagement.Commands;
using MyNotes.Application.UserManagement.Notification;
using MyNotes.Core.UserManagement;
using MyNotes.Core.UserManagement.Factories;
using MyNotes.Core.SharedKernel;
using MyNotes.Infrastructure.UserManagement.NotificationGateway;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wsa.EventBus.Contracts;

namespace MyNotes.Infrastructure.UserManagement
{
    [ExcludeFromCodeCoverage]
    public class UserManagementSetup : IBoundedContextSetup
    {
        private readonly IConfiguration _configuration;

        public UserManagementSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Register(IServiceCollection services)
        {
            services.AddTransient<UserFactory>();

            var dbConnectionString = _configuration.GetConnectionString("UserManagementDb");
            services.AddScoped(serviceProvider => new UserManagementDbContext(new ConnectionString(dbConnectionString)));

            var dbContext = services.BuildServiceProvider().GetService(typeof(UserManagementDbContext)) as UserManagementDbContext;
            dbContext?.Initialize();

            services.AddScoped<IUserManagementRepository>(
                serviceProvider => new UserManagementRepository(
                    serviceProvider.GetRequiredService<IDomainEventPublisher>(),
                    serviceProvider.GetRequiredService<UserManagementDbContext>()));
            services.AddSingleton<IUserManagementNotification>(new UserManagementNotificationSignalRGateway(NotificationHubUrl));
        }

        public void AddAuthorization(AuthorizationOptions options, string issuer)
        {
            //options.AddPolicy("read:userprofile", 
            //    policy => policy.Requirements.Add(new HasScopeRequirement("read:userprofile", issuer)));
        }

        public IEnumerable<Assembly> GetMediatRAssemblies()
        {
            yield return typeof(CreateUserCommandHandler).Assembly;
        }

        public void SubscribeToEventBus(IEventBus eventBus)
        {
            // Method intentionally left empty.
        }

        private string NotificationHubUrl => _configuration["NotificationHub"];
    }
}