using System;
using MLServer.Domain.Core.Events;

namespace MLServer.Domain.Events.Property
{
    public class PropertyRemovedEvent : Event
    {
        public PropertyRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public Guid Id { get; set; }
    }
}