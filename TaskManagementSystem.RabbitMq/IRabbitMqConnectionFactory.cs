using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.RabbitMq
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection Connection { get; }
    }
}
