using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using PropostaCredito.Infra.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace PropostaCredito.Infra.Messaging
{
    public class RabbitMQInitializer(IOptions<RabbitMQSettings> options)
    {
        private readonly RabbitMQSettings _settings = options.Value;
        private readonly AsyncRetryPolicy _retryPolicy = Policy
            .Handle<BrokerUnreachableException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) => Console.WriteLine($"Tentando conectar ao RabbitMQ. Próxima tentativa em {time.TotalSeconds} segundos."));

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
                await channel.ExchangeDeclareAsync(exchange: "propostas.events", ExchangeType.Direct, durable: true, autoDelete: false);
            });


        }
    }
}
