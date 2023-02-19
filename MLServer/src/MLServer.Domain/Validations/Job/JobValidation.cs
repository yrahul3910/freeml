using System;
using FluentValidation;
using MLServer.Domain.Commands.Job;
using MLServer.Domain.Enums;

namespace MLServer.Domain.Validations.Job
{
    public abstract class JobValidation<T> : AbstractValidator<T> where T : JobCommand
    {
        protected void ValidateId()
        {
            RuleFor(p => p.Id)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Id");
        }

        protected void ValidateName()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(1, 100).WithMessage("The Name must have between 1 and 100 characters");
        }

        protected void ValidateDescription()
        {
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(1, 256).WithMessage("The Name must have between 1 and 256 characters");
        }
        
        protected void ValidateOwner()
        {
            RuleFor(p => p.Owner)
                .NotEmpty().WithMessage("Please ensure you have entered the Owner")
                .Length(1, 50).WithMessage("The Owner must have between 1 and 50 characters")
                .EmailAddress().WithMessage("Please ensure you have entered a valid email address");
        }
        
        protected void ValidateEpochsRun()
        {
            RuleFor(p => p.EpochsRun)
                .NotNull().WithMessage("Please ensure you have entered the EpochsRun")
                .GreaterThanOrEqualTo(0).WithMessage("Please ensure you have entered a valid EpochsRun");
        }

        protected void ValidateJobStatus()
        {
            RuleFor(p => p.Status)
                .NotNull().WithMessage("Please ensure you have entered the Status")
                .IsInEnum().WithMessage("Internal system error: code 0x00");
        }
    }
}