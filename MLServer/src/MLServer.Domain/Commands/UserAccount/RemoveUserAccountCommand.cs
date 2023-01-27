using System;
using MediatR;
using MLServer.Domain.Validations.UserAccount;

namespace MLServer.Domain.Commands.UserAccount
{
    public class RemoveUserAccountCommand : UserAccountCommand, IRequest<bool>
    {
        public RemoveUserAccountCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveUserAccountCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}