using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Amqp
{
    public delegate Task AsyncAmqpHandler<TEvent>(object sender, TEvent @event) where TEvent : AmqpEventArgs;
}
