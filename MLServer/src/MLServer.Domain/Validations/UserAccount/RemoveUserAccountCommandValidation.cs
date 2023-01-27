using MLServer.Domain.Commands.UserAccount;

namespace MLServer.Domain.Validations.UserAccount
{
    public class RemoveUserAccountCommandValidation : UserAccountValidation<RemoveUserAccountCommand>
    {
        public RemoveUserAccountCommandValidation()
        {
            ValidateId();
        }
    }
}