using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwetterUserService.RabbitMQ
{
    public class RabbitMqMessenger
    {
        public void SendDeleteMessage(string userId)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq-clusterip-srv", Port = 5672 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "delete_user_queue", type: ExchangeType.Fanout);
                //channel.QueueDeclare(queue: "delete_user_queue",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                string message = userId;
                var body = Encoding.UTF8.GetBytes(message);

                //var properties = channel.CreateBasicProperties();
                //properties.Persistent = true;

                channel.BasicPublish(exchange: "delete_user_queue",
                                         routingKey: "delete_user_queue",
                                         basicProperties: null,
                                         body: body);

                channel.Close();
            }
        }
    }
}
