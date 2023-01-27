using System;
using FluentValidation;
using MLServer.Domain.Commands.UserAccount;

namespace MLServer.Domain.Validations.UserAccount
{
    public abstract class UserAccountValidation<T> : AbstractValidator<T> where T : UserAccountCommand
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
                .Length(2, 20).WithMessage("The Name must have between 2 and 20 characters");
        }
    }
}