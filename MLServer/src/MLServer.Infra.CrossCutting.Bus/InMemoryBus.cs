﻿using System.Threading.Tasks;
using MediatR;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Commands;
using MLServer.Domain.Core.Events;

namespace MLServer.Infra.CrossCutting.Bus
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public InMemoryBus(
            IMediator mediator,
            IEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        public Task<object> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            if (!@event.MessageType.Equals("DomainNotification"))
                _eventStore?.Save(@event);

            return _mediator.Publish(@event);
        }
    }
}