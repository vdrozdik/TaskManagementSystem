using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;

namespace TaskManagementSystem.RabbitMq
{
    public class RabbitMqConsumerReceive : IAmqpConsumer<RabbitMqConsumerReceive>
    {
        private readonly object _lock = new object();
        public IModel Channel { get; set; }
        public EventingBasicConsumer Consumer { get; set; }
        public IRabbitMqSettings _settings { get; set; }
        private readonly ILogger _logger;

        public event AsyncAmqpHandler<MessageAmqpEventArgs> AsyncReceived;

        public RabbitMqConsumerReceive(ILogger<RabbitMqConsumerReceive> logger, RabbitMqSettingsRecive settings, IRabbitMqConnectionFactory factory)
        {
            _logger = logger;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            (Channel, Consumer) = CreateConsumerChannel(factory.Connection, settings, OnReceived);
        }

        private (IModel, EventingBasicConsumer) CreateConsumerChannel(IConnection connection, RabbitMqSettingsRecive settings,
            EventHandler<BasicDeliverEventArgs> onReceived)
        {
            var channel = connection.CreateModel();

            channel.QueueDeclare(settings.Queue, settings.Durable, settings.Exclusive, settings.AutoDelete, settings.QueueArgumets);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += onReceived;
            channel.BasicConsume(settings.Queue, settings.AutoAck, consumer);

            return (channel, consumer); 
        }

        private void OnReceived(object sender, BasicDeliverEventArgs args)
        {
            var message = new AmqpMessage { Data = args.Body.ToArray(), Rout = args.BasicProperties.ReplyTo, ContentType = args.BasicProperties.ContentType };
            if (!string.IsNullOrEmpty(args.BasicProperties.CorrelationId))
            {
                message.CorrelationId = args.BasicProperties.CorrelationId;
            }

            Task.Run(async () =>
            {
                try
                {
                    var eventArgs = new MessageAmqpEventArgs { Message = message.Clone() };
                    await AsyncReceived?.Invoke(this, eventArgs);

                    lock (_lock)
                    {
                        if (eventArgs.IsHandled)
                            Channel.BasicAck(args.DeliveryTag, multiple: false);
                        else
                            Channel.BasicReject(args.DeliveryTag, requeue: true);
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            });
        }

        public void Dispose()
        {
            Consumer.Received -= OnReceived;
            Channel.Dispose();
        }
    }
}
