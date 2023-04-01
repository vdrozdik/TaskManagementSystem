using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Amqp
{
    public interface IRabbitMqSettings
    {
        string Queue { get; set; }
        string Exchange { get; set; }
        string Route { get; set; }
        string ExchangeNotification { get; set; }
        string RouteNotification { get; set; }
        string QueueNotification { get; set; }
        IDictionary<string, object> ExchangeArgumets { get; set; }
        IDictionary<string, object> QueueArgumets { get; set; }
        IDictionary<string, object> BindArgumets { get; set; }
        string ExchangeType { get; set; }
        byte DeliveryMode { get; set; }
        bool Durable { get; set; }
        bool Exclusive { get; set; }
        bool Mandatory { get; set; }
        bool AutoDelete { get; set; }
        bool AutoAck { get; set; }
        string messageLifeTime { get; set; }
    }

    public class RabbitMqSettingsSend : IRabbitMqSettings
    {
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string Route { get; set; }
        public string ExchangeNotification { get; set; }
        public string RouteNotification { get; set; }
        public string QueueNotification { get; set; }
        public IDictionary<string, object> ExchangeArgumets { get; set; }
        public IDictionary<string, object> QueueArgumets { get; set; }
        public IDictionary<string, object> BindArgumets { get; set; }
        public string ExchangeType { get; set; }
        public byte DeliveryMode { get; set; } = 1;
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool Mandatory { get; set; }
        public bool AutoDelete { get; set; }
        public bool AutoAck { get; set; }
        public string messageLifeTime { get; set; }
    }

    public class RabbitMqSettingsRecive : IRabbitMqSettings
    {
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string Route { get; set; }
        public string ExchangeNotification { get; set; }
        public string RouteNotification { get; set; }
        public string QueueNotification { get; set; }
        public IDictionary<string, object> ExchangeArgumets { get; set; }
        public IDictionary<string, object> QueueArgumets { get; set; }
        public IDictionary<string, object> BindArgumets { get; set; }
        public string ExchangeType { get; set; }
        public byte DeliveryMode { get; set; } = 1;
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool Mandatory { get; set; }
        public bool AutoDelete { get; set; }
        public bool AutoAck { get; set; }
        public string messageLifeTime { get; set; }
    }
}
