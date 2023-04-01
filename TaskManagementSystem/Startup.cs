using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;
using TaskManagementSystem.BusinessLayer;
using TaskManagementSystem.Model;
using TaskManagementSystem.RabbitMq;

namespace TaskManagementSystem
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagementSystem", Version = "v1" });
            });
            var rabbitMqConfigurationSettings = Configuration.GetSection("RabbitMq").Get<RabbitMqConfigurationSettings>();
            Globals.rabbitQueueRequest = rabbitMqConfigurationSettings.rabbitQueueRequest;
            var connectionSettings = CreateFactoryConnectionSettings(rabbitMqConfigurationSettings);
            var producerSettings = CreateProducerRabbitMqSettings(rabbitMqConfigurationSettings);
            services.AddSingleton(CreateConsumerSendRabbitMqSettings(rabbitMqConfigurationSettings));
            services.AddSingleton(CreateConsumerReceiveRabbitMqSettings(rabbitMqConfigurationSettings));
            services.AddSingleton<IRabbitMqConnectionFactory>(x => new RabbitMqConnectionFactory(connectionSettings));

            services.AddSingleton<IAmqpConsumer<RabbitMqConsumerReceive>, RabbitMqConsumerReceive>();
            services.AddSingleton<IAmqpConsumer<RabbitMqConsumerSend>, RabbitMqConsumerSend>();
            services.AddSingleton<IAmqpProducer<RabbitMqProducerSend>, RabbitMqProducerSend>();
            services.AddSingleton<IAmqpProducer<RabbitMqProducerReceive>, RabbitMqProducerReceive>();
            services.AddSingleton<IServiceBusHandler, ServiceBusHandler>();
            services.AddSingleton<IGlobalTaskCache, GlobalTaskCache>();

            //services.AddHostedService<ServiceBusHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagementSystem v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static RabbitMqConnectionSettings CreateFactoryConnectionSettings(RabbitMqConfigurationSettings connectionSettings)
        {
            var username = connectionSettings.rabbitlogin;
            var password = connectionSettings.rabbitPassword;
            var hostname = connectionSettings.rabbitHost;
            var port = connectionSettings.rabbitPort;
            var virtualhost = connectionSettings.rabbitVhost;

            return new RabbitMqConnectionSettings
            {
                UserName = username,
                Password = password,
                HostName = hostname.Split(';').ToList(),
                VirtualHost = virtualhost,
                Port = port,
                TopologyRecoveryEnabled = true,
                AutomaticRecoveryEnabled = true
            };
        }
        private static RabbitMqSettingsSend CreateProducerRabbitMqSettings(RabbitMqConfigurationSettings rabbitMqSettings)
        {
            return new RabbitMqSettingsSend
            {
                QueueArgumets = new Dictionary<string, object>
                {
                     { "x-queue-type", rabbitMqSettings.rabbitQueueType },
                },
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                DeliveryMode = 2
            };
        }
        private static RabbitMqSettingsSend CreateConsumerSendRabbitMqSettings(RabbitMqConfigurationSettings rabbitMqSettings)
        {
            var queue = rabbitMqSettings.rabbitQueueResponse + $".{DateTime.Now.ToString("yyyyMMdd")}-{Guid.NewGuid().ToString()}";
            Globals.rabbitReplyTo = queue;
            return new RabbitMqSettingsSend
            {
                Queue = queue,
                QueueArgumets = new Dictionary<string, object>
                {
                   { "x-queue-type", rabbitMqSettings.rabbitQueueType }
                },
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                DeliveryMode = 2
            };
        }

        private static RabbitMqSettingsRecive CreateConsumerReceiveRabbitMqSettings(RabbitMqConfigurationSettings rabbitMqSettings)
        {
            return new RabbitMqSettingsRecive
            {
                Queue = rabbitMqSettings.rabbitQueueRequest,
                QueueArgumets = new Dictionary<string, object>
                {
                   { "x-queue-type", rabbitMqSettings.rabbitQueueType }
                },
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                DeliveryMode = 2
            };
        }

    }
}
