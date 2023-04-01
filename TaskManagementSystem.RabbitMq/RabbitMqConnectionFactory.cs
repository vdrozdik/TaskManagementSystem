using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.RabbitMq
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly RabbitMqConnectionSettings _settings;

        public IConnection Connection
        {
            get
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost,
                    Port = _settings.Port,
                    AutomaticRecoveryEnabled = _settings.AutomaticRecoveryEnabled,
                    TopologyRecoveryEnabled = _settings.TopologyRecoveryEnabled
                };
                return factory.CreateConnection(_settings.HostName);
            }
        }

        public RabbitMqConnectionFactory(RabbitMqConnectionSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
    }
}
