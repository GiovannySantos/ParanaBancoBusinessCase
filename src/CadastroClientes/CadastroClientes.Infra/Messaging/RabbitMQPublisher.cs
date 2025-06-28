using CadastroClientes.Application.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CadastroClientes.Infra.Messaging
{
    public class RabbitMQPublisher : IEventPublisher
    {
        private readonly ConnectionFactory _factory;
        private readonly string _exchangeName = "cadastroClientesExchange";

        public RabbitMQPublisher(string hostName)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostName
            };
        }

        public async Task PublishClienteCadastrado(ClienteCadastradoEvent clienteCadastradoEvent)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct, durable: true);

            var routingKey = "cliente.cadastrado";
            var message = JsonConvert.SerializeObject(clienteCadastradoEvent);
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties()
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
            };

            await channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"Mensagem publicada no RabbitMQ: {JsonConvert.SerializeObject(message)}");
        }
    }
}
