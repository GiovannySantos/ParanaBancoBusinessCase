using CartaoCredito.Infra.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace CartaoCredito.Infra.Messaging
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
                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                // Declara o exchange necessário para publicar mensagens
                await channel.ExchangeDeclareAsync(exchange: "cartao.credito.events", ExchangeType.Direct, durable: true, autoDelete: false);

                // Declara a fila necessária para receber mensagens
                await channel.QueueDeclareAsync(queue: "cartao.proposta.aprovada.queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                
                // Liga a fila ao exchange com a chave de roteamento
                await channel.QueueBindAsync(queue: "cartao.proposta.aprovada.queue", exchange: "proposta.credito.events", routingKey: "proposta.aprovada");
            });


        }
    }
}