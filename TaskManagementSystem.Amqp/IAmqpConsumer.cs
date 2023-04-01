using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Amqp
{
    public interface IAmqpConsumer<T> : IDisposable
    {
        event AsyncAmqpHandler<MessageAmqpEventArgs> AsyncReceived;
        EventingBasicConsumer Consumer { get; set; }
        IModel Channel { get; set; }
        IRabbitMqSettings _settings { get; set; }
    }
}
