using OnlineStore.Api.Application.Orders.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Azure.Interfaces
{
    public interface IQueueStorage
    {
        Task SetupQueueAsync(string queueName);
        Task CreateAsync(string queueName, string message, IOrderQueueService orderQueueSerivce, CancellationToken cancellation = default);
    }
}
