using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.RabbitMq
{
    public class RabbitMqConfigurationSettings
    {
        public string rabbitlogin { get; set; }
        public string rabbitPassword { get; set; }
        public string rabbitHost { get; set; }
        public string rabbitVhost { get; set; }
        public int rabbitPort { get; set; }
        public string rabbitQueueRequest { get; set; }
        public string rabbitQueueResponse { get; set; }
        public string rabbitQueueType { get; set; }
        public string ttlRequestMessage { get; set; }
    }
}
