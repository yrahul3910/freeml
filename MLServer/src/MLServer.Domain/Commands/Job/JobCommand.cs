using System;
using MLServer.Domain.Core.Commands;
using MLServer.Domain.Enums;
using Type = MLServer.Domain.Enums.Type;

namespace MLServer.Domain.Commands.Job
{
    public abstract class JobCommand : Command
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public JobStatus Status { get; protected set; }
    }
}