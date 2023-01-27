using System.Threading.Tasks;
using MLServer.Domain.Core.Commands;
using MLServer.Domain.Core.Events;

namespace MLServer.Domain.Core.Bus
{
    public interface IMediatorHandler
    {
        Task<object> SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}