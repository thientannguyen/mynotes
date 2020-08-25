using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MyNotes.Infrastructure.BoundedContext.NotificationGateway;
using MyNotes.Infrastructure;
using Invio.Extensions.Authentication.JwtBearer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyNotes.API.Authentication;
using MyNotes.API.Hubs;
using MyNotes.Application.BoundedContext;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.SharedKernel;
using MyNotes.Infrastructure.UserManagement;
using Wsa.EventBus.Contracts;

namespace MyNotes.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBoundedContextSetup[] _boundedContextSetups;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
            _boundedContextSetups = new IBoundedContextSetup[]
            {
                new UserManagementSetup(Configuration)
            };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                var policyBuilder = new CorsPolicyBuilder();
                policyBuilder
                    //.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();

                if (!string.IsNullOrEmpty(Configuration["CorsAllowSpecific"]))
                {
                    policyBuilder.WithOrigins(Configuration["CorsAllowSpecific"].Split(","));
                }
                else
                {
                    policyBuilder.AllowAnyOrigin();
                }

                options.AddPolicy("CorsPolicy", policyBuilder.Build());
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 100000;
            });

            var notificationHubUrl = Configuration["NotificationHub"];
            services.AddSingleton<IBoundedContextNotification>(
                serviceProvider => new BoundedContextSignalRGateway(
                    notificationHubUrl,
                    serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("NotificationService")));
            services.AddMvc()
                .AddMvcOptions(options => options.EnableEndpointRouting = true)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers()
                .AddNewtonsoftJson();

            //Allows auth to be bypassed
            //if (_env.IsEnvironment("Local"))
            //{ services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();}

            services.AddSingleton<IAuthorizationHandler>(new ApiKeyHandler(new HttpContextAccessor()));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TestApiKey", policy =>
                    policy.Requirements.Add(new ApiKeyRequirement("x-test-api-key", Configuration["TestApiKey"])));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority =
                    $"https://{Configuration["AzureAdB2C:Tenant"]}.b2clogin.com/{Configuration["AzureAdB2C:TenantId"]}/{Configuration["AzureAdB2C:SignUpSignInPolicyId"]}";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudiences = new List<string> { Configuration["AzureAdB2C:Audience"] },
                    ValidIssuers = new List<string>
                    {
                        $"https://{Configuration["AzureAdB2C:Tenant"]}.b2clogin.com/{Configuration["AzureAdB2C:TenantId"]}/v2.0/"
                    }
                };
                options.AddQueryStringAuthentication();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "NextGen APi" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                c.CustomSchemaIds(x => x.FullName);
            });

            AddMediatr(services);
            SetupInfrastructure(services);
            ForEachBoundedContextsDo(setup => setup.Register(services));
            var eventBus = new EventBus(services);
            services.AddSingleton<IEventBus>(sp => eventBus);
            foreach (var context in _boundedContextSetups)
            {
                context.SubscribeToEventBus(eventBus);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "NextGen API V1"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/NotificationHub");
                endpoints.MapControllers();
            });
        }

        private void AddMediatr(IServiceCollection services)
        {
            services.AddMediatR(_boundedContextSetups.SelectMany(setup => setup.GetMediatRAssemblies()).ToArray());
        }

        private void ForEachBoundedContextsDo(Action<IBoundedContextSetup> @do)
        {
            foreach (var setup in _boundedContextSetups)
            {
                @do(setup);
            }
        }

        private void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        }
    }
}
