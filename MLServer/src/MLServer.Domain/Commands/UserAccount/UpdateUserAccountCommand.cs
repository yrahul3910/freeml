using System;
using System.Xml.Linq;
using MediatR;
using MLServer.Domain.Validations.UserAccount;

namespace MLServer.Domain.Commands.UserAccount
{
    public class UpdateUserAccountCommand : UserAccountCommand, IRequest<bool>
    {
        public UpdateUserAccountCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserAccountCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}