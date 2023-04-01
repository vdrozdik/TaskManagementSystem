using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;

namespace TaskManagementSystem.BusinessLayer
{
    public interface IServiceBusHandler
    {
        Task<AmqpMessage> SendMessage(AmqpMessage message);
    }
}
