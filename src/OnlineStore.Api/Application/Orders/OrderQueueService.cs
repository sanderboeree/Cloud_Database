using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using OnlineStore.Api.Application.Orders.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Azure.Interfaces;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Orders
{
    public class OrderQueueService : IOrderQueueService
    {
        private readonly string queueName = "orders";

        private readonly IQueueStorage _queueStorage;
        private readonly ICrudService<Order, OrderDto> _ordersCrudService;
        private readonly ICrudService<OrderProduct, OrderProductDto> _orderProductsCrudService;

        public OrderQueueService(IQueueStorage queueStorage,
            ICrudService<Order, OrderDto> ordersCrudService,
            ICrudService<OrderProduct, OrderProductDto> orderProductsCrudService)
        {
            _queueStorage = queueStorage;
            _ordersCrudService = ordersCrudService;
            _orderProductsCrudService = orderProductsCrudService;
        }

        public async Task CreateAsync(OrderDto order, CancellationToken cancellationToken = default)
        {
            var message = JsonConvert.SerializeObject(order);
            await _queueStorage.CreateAsync(queueName, message, this, cancellationToken);
        }

        public async Task ProcessAsync(QueueMessage message, CancellationToken cancellationToken = default)
        {
            var dto = JsonConvert.DeserializeObject<OrderDto>(message.Body.ToString());
            var order = await _ordersCrudService.CreateAsync(dto, cancellationToken);

            foreach (var orderProduct in dto.OrderProducts)
            {
                orderProduct.OrderId = order.Id;
                await _orderProductsCrudService.CreateAsync(orderProduct, cancellationToken);
            }
        }
    }
}
