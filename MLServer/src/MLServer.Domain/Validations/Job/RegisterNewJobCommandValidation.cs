using MLServer.Domain.Commands.Job;

namespace MLServer.Domain.Validations.Job
{
    public class RegisterNewJobCommandValidation : JobValidation<RegisterNewJobCommand>
    {
        public RegisterNewJobCommandValidation()
        {
            ValidateName();
            ValidateDescription();
            ValidateJobStatus();
        }
    }
}