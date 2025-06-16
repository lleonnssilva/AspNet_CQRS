using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace AspNet_CQRS.Application.Members.Messages
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConfiguration _configuration;
        public RabbitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }

        public void SendMemberMessage<T>(T message)
        {

            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetConnectionString("RmQConnection")
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("member", exclusive: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "member", body: body);


        }
    }
}
