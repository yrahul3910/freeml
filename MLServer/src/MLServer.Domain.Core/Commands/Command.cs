using System;
using FluentValidation.Results;
using MLServer.Domain.Core.Events;

namespace MLServer.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; }
        public ValidationResult ValidationResult { get; set; }

        public abstract bool IsValid();
    }
}