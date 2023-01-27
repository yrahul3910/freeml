﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MLServer.Domain.Events.Property;

namespace MLServer.Domain.EventHandlers
{
    public class PropertyEventHandler :
        INotificationHandler<PropertyRegisteredEvent>,
        INotificationHandler<PropertyUpdatedEvent>,
        INotificationHandler<PropertyRemovedEvent>
    {
        public Task Handle(PropertyRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(PropertyUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(PropertyRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}