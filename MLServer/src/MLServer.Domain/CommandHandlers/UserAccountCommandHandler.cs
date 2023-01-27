using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MLServer.Domain.Commands.UserAccount;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Notifications;
using MLServer.Domain.Interfaces;
using MLServer.Domain.Models;

namespace MLServer.Domain.CommandHandlers
{
    public class UserAccountCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewUserAccountCommand, object>,
        IRequestHandler<UpdateUserAccountCommand, bool>,
        IRequestHandler<RemoveUserAccountCommand, bool>
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IMediatorHandler _bus;

        public UserAccountCommandHandler(
            IUserAccountRepository userAccountRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications)
            : base(uow, bus, notifications)
        {
            _userAccountRepository = userAccountRepository;
            _bus = bus;
        }

        public async Task<object> Handle(RegisterNewUserAccountCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return await Task.FromResult(false);
            }

            var userAccount = new UserAccount(Guid.NewGuid(), message.Name);

            _userAccountRepository.Add(userAccount);

            if (Commit())
            {
            }

            return await Task.FromResult(userAccount);
        }

        public Task<bool> Handle(UpdateUserAccountCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var userAccount = _userAccountRepository.GetById(message.Id).Update(message.Name);

            _userAccountRepository.Update(userAccount);

            if (Commit())
            {
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveUserAccountCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _userAccountRepository.Remove(message.Id);

            if (Commit())
            {
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _userAccountRepository.Dispose();
        }
    }
}