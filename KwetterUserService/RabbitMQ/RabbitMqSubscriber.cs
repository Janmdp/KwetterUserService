using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace KwetterUserService.RabbitMQ
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private readonly UserLogic logic;
        private readonly IServiceScopeFactory scopeFactory;

        public RabbitMqSubscriber(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            logic = new UserLogic(dbContext);
            StartClient();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //todo put own logic here
                User user = JsonConvert.DeserializeObject<User>(message);
                logic.CreateUser(user);
            };
            channel.BasicConsume(queue: "task_queue",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        private void StartClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 49154 };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            
                channel.ExchangeDeclare(exchange: "task_queue", type: ExchangeType.Fanout);

                channel.QueueDeclare(queue: "task_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: "task_queue", exchange: "task_queue", routingKey: "");
            
        }

        public override void Dispose()
        {
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
            base.Dispose();
        }


    }
}
