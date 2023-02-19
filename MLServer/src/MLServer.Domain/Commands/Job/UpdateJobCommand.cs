using System;
using MediatR;
using MLServer.Domain.Enums;
using MLServer.Domain.Validations.Job;
using Type = MLServer.Domain.Enums.Type;

namespace MLServer.Domain.Commands.Job
{
    public class UpdateJobCommand : JobCommand, IRequest<bool>
    {
        public UpdateJobCommand(Guid id, string name, string description, JobStatus status, string owner)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Owner = owner;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateJobCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}