using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Amqp
{
    public class AmqpMessage
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
        public byte[] Data { get; set; }
        public string ContentType { get; set; } = "application/json";
        public string Rout { get; set; }
        public string RabbitQueueRequest { get; set; }
        public virtual AmqpMessage Clone()
        {
            var clone = new AmqpMessage
            {
                CorrelationId = CorrelationId,

                Rout = Rout,
                ContentType = ContentType,
                RabbitQueueRequest = RabbitQueueRequest,
            };

            if (this.Data == null)
            {
                clone.Data = new byte[0];
                return clone;
            }

            clone.Data = new byte[Data.Length];
            Array.Copy(Data, clone.Data, Data.Length);
            return clone;
        }
    }
}
