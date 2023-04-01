using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;

using TaskManagementSystem.RabbitMq;

namespace TaskManagementSystem.BusinessLayer
{
    public class ServiceBusHandler : IServiceBusHandler
    {
        private readonly IAmqpConsumer<RabbitMqConsumerReceive> _consumerReceive;
        private readonly IAmqpProducer<RabbitMqProducerReceive> _producerReceive;
        private readonly ILogger _logger;
        private readonly IAmqpConsumer<RabbitMqConsumerSend> _consumerSend;
        private readonly IAmqpProducer<RabbitMqProducerSend> _producerSend;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<AmqpMessage>> _pendingMessages;
        public ServiceBusHandler(IAmqpConsumer<RabbitMqConsumerSend> consumerSend, IAmqpProducer<RabbitMqProducerSend> producerSend, IAmqpProducer<RabbitMqProducerReceive> producerReceive, IAmqpConsumer<RabbitMqConsumerReceive> consumerReceive, ILogger<ServiceBusHandler> logger)
        {
            _producerReceive = producerReceive;
            _consumerReceive = consumerReceive;
            _consumerSend = consumerSend;
            _producerSend = producerSend;
            _logger = logger;
            _pendingMessages = new ConcurrentDictionary<string, TaskCompletionSource<AmqpMessage>>();
            _consumerSend.AsyncReceived += OnReceived;
            ReceiveMessage();
        }
        public void ReceiveMessage()
        {
            _consumerReceive.AsyncReceived += async (sender, e) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.Message.Data.ToArray()));
                _producerReceive.Publish(new AmqpMessage
                {
                    Rout = e.Message.Rout,
                    CorrelationId = e.Message.CorrelationId,
                    Data = Encoding.UTF8.GetBytes("ok"),
                    ContentType = "application/json"
                });
            }; 
        }

        private async Task OnReceived(object sender, MessageAmqpEventArgs @event)
        {
            if (_pendingMessages.TryRemove(@event.Message.CorrelationId, out var tcs))
            {
                tcs.SetResult(@event.Message);
            }
        }

        public Task<AmqpMessage> SendMessage(AmqpMessage message)
        {
            var tcs = new TaskCompletionSource<AmqpMessage>();
            _pendingMessages[message.CorrelationId] = tcs;

            _producerSend.Publish(message);

            return tcs.Task;
        }

        public void Dispose()
        {
            _consumerSend.AsyncReceived -= OnReceived;
            _consumerSend.Dispose();
        }

        /// <summary>
        /// Обработчик полученых данных с брокера сообщений Rabbit
        /// </summary>

    }
}
