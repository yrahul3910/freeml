using System;
using Microsoft.AspNetCore.Http;
using MLServer.Domain.Core.Commands;
using MLServer.Domain.Enums;

namespace MLServer.Domain.Commands.Job
{
    public abstract class JobCommand : Command
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int EpochsRun { get; protected set; }
        public string Owner { get; protected set; }
        public JobStatus Status { get; protected set; }
        public IFormFile Model { get; protected set; }
        public IFormFile Dataset { get; protected set; }
    }
}