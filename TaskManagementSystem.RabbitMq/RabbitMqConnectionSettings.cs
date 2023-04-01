using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.RabbitMq
{
    public class RabbitMqConnectionSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public bool TopologyRecoveryEnabled { get; set; }
    }
}
