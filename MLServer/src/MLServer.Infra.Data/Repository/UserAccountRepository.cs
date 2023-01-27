using MLServer.Domain.Interfaces;
using MLServer.Domain.Models;
using MLServer.Infra.Data.Context;

namespace MLServer.Infra.Data.Repository
{
    public class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(MLServerContext context)
            : base(context)
        {
        }
    }
}