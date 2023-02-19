using MediatR;
using MLServer.Domain.Enums;
using MLServer.Domain.Validations.Job;
using System;

namespace MLServer.Domain.Commands.Job
{
    public class RegisterNewJobCommand : JobCommand, IRequest<object>
    {
        public RegisterNewJobCommand(string name, string description, JobStatus status, int epochsRun, string owner)
        {
            Name = name;
            Description = description;
            Status = status;
            EpochsRun = epochsRun;
            Owner = owner;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewJobCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}