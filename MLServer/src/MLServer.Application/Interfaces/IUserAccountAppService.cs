using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MLServer.Application.ViewModels.v1.UserAccount;

namespace MLServer.Application.Interfaces
{
    public interface IUserAccountAppService : IDisposable
    {
        IEnumerable<UserAccountViewModel> GetAll();
        UserAccountViewModel GetById(Guid id);
        Task<object> Register(RegisterNewUserAccountViewModel registerNewUserAccountViewModel);
        void Update(UpdateUserAccountViewModel updateUserAccountViewModel);
        void Remove(Guid id);
    }
}