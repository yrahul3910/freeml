using MLServer.Domain.Commands.Job;

namespace MLServer.Domain.Validations.Job
{
    public class RemoveJobCommandValidation : JobValidation<RemoveJobCommand>
    {
        public RemoveJobCommandValidation()
        {
            ValidateId();
        }
    }
}