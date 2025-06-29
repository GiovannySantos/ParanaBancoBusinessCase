using CadastroClientes.Infra.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace CadastroClientes.Infra.Messaging
{
    public class RabbitMQInitializer
    {
        private readonly RabbitMQSettings _settings;
        private readonly AsyncRetryPolicy _retryPolicy;

        public RabbitMQInitializer(IOptions<RabbitMQSettings> options)
        {
            _settings = options.Value;

            _retryPolicy = Policy
            .Handle<BrokerUnreachableException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => Console.WriteLine($"Tentando conectar ao RabbitMQ. Próxima tentativa em {time.TotalSeconds} segundos."));
        }

        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName
            };

            await _retryPolicy.ExecuteAsync(async () =>
            {
                // Tenta criar uma conexão com o RabbitMQ
                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                // Declara o exchange necessário
                await channel.ExchangeDeclareAsync(exchange: "cadastro.clientes.events", ExchangeType.Direct, durable: true, autoDelete: false);
            });

            
        }
    }
}