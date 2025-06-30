using CadastroClientes.Infra.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace CadastroClientes.Infra.Messaging
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
                await channel.ExchangeDeclareAsync(exchange: "proposta.credito.events", ExchangeType.Direct, durable: true, autoDelete: false);
                await channel.ExchangeDeclareAsync(exchange: "cadastro.clientes.events", ExchangeType.Direct, durable: true, autoDelete: false);
                await channel.ExchangeDeclareAsync(exchange: "cartao.credito.events", ExchangeType.Direct, durable: true, autoDelete: false);

                // Declara as fila necessárias para receber mensagens
                await channel.QueueDeclareAsync(queue: "cartao.criado.queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                await channel.QueueDeclareAsync(queue: "proposta.credito.falhou.queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                await channel.QueueDeclareAsync(queue: "cartao.credito.falhou.queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Liga a fila ao exchange com a chave de roteamento
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await channel.QueueBindAsync(queue: "cartao.criado.queue", exchange: "cartao.credito.events", routingKey: "cartao.credito.criado");
                    await channel.QueueBindAsync("proposta.credito.falhou.queue", "proposta.credito.events", "proposta.credito.falhou");
                    await channel.QueueBindAsync("cartao.credito.falhou.queue", "cartao.credito.events", "cartao.credito.falhou");
                });
            });


        }
    }
}