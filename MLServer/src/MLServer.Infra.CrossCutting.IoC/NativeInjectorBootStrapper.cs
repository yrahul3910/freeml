using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MLServer.Application.Interfaces;
using MLServer.Application.Services;
using MLServer.Domain.CommandHandlers;
using MLServer.Domain.Commands.Job;
using MLServer.Domain.Commands.UserAccount;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Events;
using MLServer.Domain.Core.Notifications;
using MLServer.Domain.EventHandlers;
using MLServer.Domain.Events.Property;
using MLServer.Domain.Interfaces;
using MLServer.Infra.CrossCutting.Bus;
using MLServer.Infra.CrossCutting.Identity.Authorization;
using MLServer.Infra.CrossCutting.Identity.Models;
using MLServer.Infra.Data.Context;
using MLServer.Infra.Data.EventSourcing;
using MLServer.Infra.Data.Repository;
using MLServer.Infra.Data.Repository.EventSourcing;
using MLServer.Infra.Data.UoW;

namespace MLServer.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<IJobAppService, JobAppService>();
            services.AddScoped<IUserAccountAppService, UserAccountAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Property
            services.AddScoped<INotificationHandler<PropertyRegisteredEvent>, PropertyEventHandler>();
            services.AddScoped<INotificationHandler<PropertyUpdatedEvent>, PropertyEventHandler>();
            services.AddScoped<INotificationHandler<PropertyRemovedEvent>, PropertyEventHandler>();

            // Domain - Commands

            // UserAccount
            services.AddScoped<IRequestHandler<RegisterNewUserAccountCommand, object>, UserAccountCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateUserAccountCommand, bool>, UserAccountCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveUserAccountCommand, bool>, UserAccountCommandHandler>();

            // Job
            services.AddScoped<IRequestHandler<RegisterNewJobCommand, object>, JobCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateJobCommand, bool>, JobCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveJobCommand, bool>, JobCommandHandler>();

            // Infra - Data
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<MLServerContext>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSqlContext>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}