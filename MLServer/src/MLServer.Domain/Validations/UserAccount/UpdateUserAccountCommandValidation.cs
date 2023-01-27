using MLServer.Domain.Commands.UserAccount;

namespace MLServer.Domain.Validations.UserAccount
{
    public class UpdateUserAccountCommandValidation : UserAccountValidation<UpdateUserAccountCommand>
    {
        public UpdateUserAccountCommandValidation()
        {
            ValidateId();
            ValidateName();
        }
    }
}