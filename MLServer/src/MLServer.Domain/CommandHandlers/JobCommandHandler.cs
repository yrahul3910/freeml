using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MLServer.Domain.Commands.Job;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Events;
using MLServer.Domain.Core.Notifications;
using MLServer.Domain.Interfaces;
using MLServer.Domain.Models;

namespace MLServer.Domain.CommandHandlers
{
    public class JobCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewJobCommand, object>,
        IRequestHandler<UpdateJobCommand, bool>,
        IRequestHandler<RemoveJobCommand, bool>
    {
        private readonly IJobRepository _JobRepository;
        private readonly IMediatorHandler _bus;

        public JobCommandHandler(
            IJobRepository JobRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications)
            : base(uow, bus, notifications)
        {
            _JobRepository = JobRepository;
            _bus = bus;
        }

        private async Task<string> CreateJobDirectory(string id, string name)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "jobs", id, name);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return await Task.FromResult(path);
        }

        public async Task<object> Handle(RegisterNewJobCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return await Task.FromResult(false);
            }

            // Copy the model over.
            var path = await this.CreateJobDirectory(message.Id.ToString(), "model");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await message.Model.CopyToAsync(stream);
            }

            path = await this.CreateJobDirectory(message.Id.ToString(), "data");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await message.Dataset.CopyToAsync(stream);
            }

            var Job = new Job(Guid.NewGuid(), message.Name, message.Description, message.Status);

            _JobRepository.Add(Job);

            if (Commit())
            {
            }

            return await Task.FromResult(Job);
        }

        public Task<bool> Handle(UpdateJobCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var Job = _JobRepository.GetById(message.Id).Update(message.Name, message.Description, message.Status);

            _JobRepository.Update(Job);

            if (Commit())
            {
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveJobCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _JobRepository.Remove(message.Id);

            if (Commit())
            {
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _JobRepository.Dispose();
        }
    }
}