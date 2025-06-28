using CadastroClientes.Infra.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CadastroClientes.Infra.Messaging
{
    public class RabbitMQInitializer
    {
        private readonly RabbitMQSettings _settings;

        public RabbitMQInitializer(IOptions<RabbitMQSettings> options)
        {
            _settings = options.Value;
        }

        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            // Declarar a exchange
            await channel.ExchangeDeclareAsync(exchange: "cadastro.clientes.events", ExchangeType.Direct, durable: true, autoDelete: false);
        }
    }
}