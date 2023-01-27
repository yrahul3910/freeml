using MediatR;
using MLServer.Domain.Enums;
using MLServer.Domain.Validations.UserAccount;
using System;

namespace MLServer.Domain.Commands.UserAccount
{
    public class RegisterNewUserAccountCommand : UserAccountCommand, IRequest<object>
    {
        public RegisterNewUserAccountCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewUserAccountCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}