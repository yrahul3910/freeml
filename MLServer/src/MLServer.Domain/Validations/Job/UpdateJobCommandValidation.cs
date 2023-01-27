using MLServer.Domain.Commands.Job;

namespace MLServer.Domain.Validations.Job
{
    public class UpdateJobCommandValidation : JobValidation<UpdateJobCommand>
    {
        public UpdateJobCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateJobStatus();
        }
    }
}