using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;

namespace TaskManagementSystem.RabbitMq
{
    public interface IRpcAmqpClient : IDisposable
    {
        Task<AmqpMessage> SendAsync(AmqpMessage message);
    }
}
