using System;
using MLServer.Domain.Core.Commands;
using Type = MLServer.Domain.Enums.Type;

namespace MLServer.Domain.Commands.UserAccount
{
    public abstract class UserAccountCommand : Command
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
    }
}