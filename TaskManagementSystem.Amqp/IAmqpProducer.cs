using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Amqp
{
    public interface IAmqpProducer<T> : IDisposable
    {
        void Publish(AmqpMessage message);
    }
}
