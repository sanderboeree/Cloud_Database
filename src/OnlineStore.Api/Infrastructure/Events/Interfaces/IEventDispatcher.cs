using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Events.Interfaces
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(IEvent evt, CancellationToken cancellationToken = default);
    }
}
