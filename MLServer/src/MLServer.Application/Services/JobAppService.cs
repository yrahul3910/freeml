using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MLServer.Application.Interfaces;
using MLServer.Application.ViewModels.v1.Job;
using MLServer.Domain.Commands.Job;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Notifications;
using MLServer.Domain.Interfaces;
using MLServer.Domain.Models;

namespace MLServer.Application.Services
{
    public class JobAppService : IJobAppService
    {
        private readonly IMapper _mapper;
        private readonly IJobRepository _jobRepository;
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;

        public JobAppService(
            IMapper mapper,
            IJobRepository jobRepository,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications)
        {
            _mapper = mapper;
            _jobRepository = jobRepository;
            _bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
        }

        public IEnumerable<JobViewModel> GetAll()
        {
            return _jobRepository.GetAll().ProjectTo<JobViewModel>(_mapper.ConfigurationProvider);
        }

        public JobViewModel GetById(Guid id)
        {
            return _mapper.Map<JobViewModel>(_jobRepository.GetById(id));
        }

        public async Task<object> Register(RegisterNewJobViewModel registerNewJobViewModel)
        {
            var registerCommand = _mapper.Map<RegisterNewJobCommand>(registerNewJobViewModel);
            var registerResponse = await _bus.SendCommand(registerCommand);

            return _notifications.HasNotifications() ? registerResponse : _mapper.Map<JobViewModel>((Job)registerResponse);
        }

        public void Update(UpdateJobViewModel updateJobViewModel)
        {
            var updateCommand = _mapper.Map<UpdateJobCommand>(updateJobViewModel);
            _bus.SendCommand(updateCommand);
        }

        public void Remove(Guid id)
        {
            var removeCommand = new RemoveJobCommand(id);
            _bus.SendCommand(removeCommand);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}