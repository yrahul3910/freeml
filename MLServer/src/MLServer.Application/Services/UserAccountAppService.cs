using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MLServer.Application.Interfaces;
using MLServer.Application.ViewModels.v1.UserAccount;
using MLServer.Domain.Commands.UserAccount;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Notifications;
using MLServer.Domain.Interfaces;
using MLServer.Domain.Models;

namespace MLServer.Application.Services
{
    public class UserAccountAppService : IUserAccountAppService
    {
        private readonly IMapper _mapper;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;

        public UserAccountAppService(
            IMapper mapper,
            IUserAccountRepository userAccountRepository,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications)
        {
            _mapper = mapper;
            _userAccountRepository = userAccountRepository;
            _bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
        }

        public IEnumerable<UserAccountViewModel> GetAll()
        {
            return _userAccountRepository.GetAll().ProjectTo<UserAccountViewModel>(_mapper.ConfigurationProvider);
        }

        public UserAccountViewModel GetById(Guid id)
        {
            return _mapper.Map<UserAccountViewModel>(_userAccountRepository.GetById(id));
        }

        public async Task<object> Register(RegisterNewUserAccountViewModel registerNewUserAccountViewModel)
        {
            var registerCommand = _mapper.Map<RegisterNewUserAccountCommand>(registerNewUserAccountViewModel);
            var registerResponse = await _bus.SendCommand(registerCommand);

            return _notifications.HasNotifications() ? registerResponse : _mapper.Map<UserAccountViewModel>((UserAccount)registerResponse);
        }

        public void Update(UpdateUserAccountViewModel updateUserAccountViewModel)
        {
            var updateCommand = _mapper.Map<UpdateUserAccountCommand>(updateUserAccountViewModel);
            _bus.SendCommand(updateCommand);
        }

        public void Remove(Guid id)
        {
            var removeCommand = new RemoveUserAccountCommand(id);
            _bus.SendCommand(removeCommand);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}