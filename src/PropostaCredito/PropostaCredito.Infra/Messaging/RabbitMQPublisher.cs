using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Application.Messaging;
using PropostaCredito.Infra.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace PropostaCredito.Infra.Messaging
{
    public class RabbitMQPublisher : IEventPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQPublisher(IOptions<RabbitMQSettings> options)
        {
            _factory = new ConnectionFactory
            {
                HostName = options.Value.HostName,
            };
        }

        public async Task PublishAsync<T>(T evento, PublishProperties publishProperties)
        {
            try
            {
                using IConnection connection = await _factory.CreateConnectionAsync();
                using IChannel channel = await connection.CreateChannelAsync();

                var message = JsonConvert.SerializeObject(evento);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = new BasicProperties
                {
                    ContentType = "application/json",
                    DeliveryMode = DeliveryModes.Persistent,
                    MessageId = Guid.NewGuid().ToString(),
                    CorrelationId = Guid.NewGuid().ToString()
                };

                await channel.BasicPublishAsync(publishProperties.Exchange, publishProperties.RoutingKey, publishProperties.Mandatory, properties, body);
            }
            catch (BrokerUnreachableException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

