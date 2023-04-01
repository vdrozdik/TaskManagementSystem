using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Amqp
{
    public class MessageAmqpEventArgs : AmqpEventArgs
    {
        public AmqpMessage Message { get; set; }

        /// <summary>
        /// Признак необходимости удаления сообщения из очереди после обработки
        /// </summary>
        public bool IsHandled { get; set; } = true;
    }
}
