using Azure.Storage.Queues.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Orders.Interfaces
{
    public interface IOrderQueueService
    {
        Task CreateAsync(OrderDto order, CancellationToken cancellationToken = default);
        Task ProcessAsync(QueueMessage message, CancellationToken cancellationToken = default);
    }
}
