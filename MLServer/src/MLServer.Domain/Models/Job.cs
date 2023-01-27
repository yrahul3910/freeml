using System;
using MLServer.Domain.Core.Models;
using MLServer.Domain.Enums;

namespace MLServer.Domain.Models
{
    public class Job : Entity
    {
        public Job(Guid id, string name, string description, JobStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
        }

        // Empty constructor for EF
        protected Job()
        {
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public JobStatus Status { get; private set; }

        public Job Update(string name, string description, JobStatus status)
        {
            Name = name;
            Description = description;
            Status = status;

            return this;
        }
    }
}