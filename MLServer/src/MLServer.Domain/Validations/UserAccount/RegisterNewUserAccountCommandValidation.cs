using MLServer.Domain.Commands.UserAccount;

namespace MLServer.Domain.Validations.UserAccount
{
    public class RegisterNewUserAccountCommandValidation : UserAccountValidation<RegisterNewUserAccountCommand>
    {
        public RegisterNewUserAccountCommandValidation()
        {
            ValidateName();
        }
    }
}