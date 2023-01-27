using MLServer.Domain.Interfaces;
using MLServer.Domain.Models;
using MLServer.Infra.Data.Context;

namespace MLServer.Infra.Data.Repository
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(MLServerContext context)
            : base(context)
        {
        }
    }
}