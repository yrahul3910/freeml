using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MLServer.Application.ViewModels.v1.Job;

namespace MLServer.Application.Interfaces
{
    public interface IJobAppService : IDisposable
    {
        IEnumerable<JobViewModel> GetAll();
        JobViewModel GetById(Guid id);
        Task<object> Register(RegisterNewJobViewModel registerNewJobViewModel);
        void Update(UpdateJobViewModel updateJobViewModel);
        void Remove(Guid id);
    }
}