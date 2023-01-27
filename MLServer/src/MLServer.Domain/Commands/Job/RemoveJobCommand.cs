using System;
using MediatR;
using MLServer.Domain.Validations.Job;

namespace MLServer.Domain.Commands.Job
{
    public class RemoveJobCommand : JobCommand, IRequest<bool>
    {
        public RemoveJobCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveJobCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}