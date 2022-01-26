using Azure.Storage.Queues;
using OnlineStore.Api.Infrastructure.Azure.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Orders.Interfaces;

namespace OnlineStore.Api.Infrastructure.Azure
{
    public class QueueStorage : IQueueStorage
    {
        private readonly QueueServiceClient _queueServiceClient;

        public QueueStorage(QueueServiceClient queueServiceClient)
        {
            _queueServiceClient = queueServiceClient;
        }

        public async Task SetupQueueAsync(string queueName)
        {
            await _queueServiceClient.GetQueueClient(queueName).CreateIfNotExistsAsync();
        }

        public async Task CreateAsync(string queueName, string message, IOrderQueueService orderQueueService, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            var queueClient = GetQueueClient(queueName);
            await queueClient.SendMessageAsync(message, cancellationToken);

            foreach (var queueMessage in queueClient.ReceiveMessages(25, TimeSpan.FromSeconds(60), cancellationToken).Value)
            {
                //Should have a base service when there are multiple queues
                await orderQueueService.ProcessAsync(queueMessage, cancellationToken);
                await queueClient.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt, cancellationToken);
            }
        }

        private QueueClient GetQueueClient(string queueName)
        {
            return _queueServiceClient.GetQueueClient(queueName);
        }
    }
}
