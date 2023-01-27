using MediatR;
using MLServer.Domain.Enums;
using MLServer.Domain.Validations.Job;
using System;

namespace MLServer.Domain.Commands.Job
{
    public class RegisterNewJobCommand : JobCommand, IRequest<object>
    {
        public RegisterNewJobCommand(string name, string description, JobStatus status)
        {
            Name = name;
            Description = description;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewJobCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}