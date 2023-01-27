using System;
using MLServer.Domain.Core.Events;
using Type = MLServer.Domain.Enums.Type;

namespace MLServer.Domain.Events.Property
{
    public class PropertyRegisteredEvent : Event
    {
        public PropertyRegisteredEvent(Guid id, string name, Type type, bool isRequired)
        {
            Id = id;
            Name = name;
            Type = type;
            IsRequired = isRequired;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
        public Type Type { get; }
        public bool IsRequired { get; }
    }
}