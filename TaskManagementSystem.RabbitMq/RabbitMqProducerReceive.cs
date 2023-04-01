﻿using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;

namespace TaskManagementSystem.RabbitMq
{
    public class RabbitMqProducerReceive : IAmqpProducer<RabbitMqProducerReceive>
    {
        private readonly object _lock = new object();
        private IModel _channel;
        private readonly IRabbitMqSettings _settings;
        private IRabbitMqConnectionFactory factory;
        private static ILogger logger;
        public RabbitMqProducerReceive(RabbitMqSettingsRecive settings, IRabbitMqConnectionFactory factory, ILogger<RabbitMqProducerReceive> _logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.factory = factory;
            logger = _logger;
            _channel = CreateProducerChannel(factory?.Connection ?? throw new ArgumentNullException(nameof(factory)), _settings);

        }

        private static IModel CreateProducerChannel(IConnection connection, IRabbitMqSettings settings)
        {
            var channel = connection.CreateModel();
            return channel;
        }

        public void Publish(AmqpMessage message)
        {
            var props = _channel.CreateBasicProperties();

            props.CorrelationId = message.CorrelationId;
            props.DeliveryMode = _settings.DeliveryMode;

            lock (_lock)
                _channel.BasicPublish("", message.Rout, _settings.Mandatory, props, message.Data);

        }

        public void Dispose()
        {
            if (_channel != null)
                _channel.Dispose();
        }
    }
}
