using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Domain.Events.Consumers;
using PropostaCredito.Infra.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PropostaCredito.API.Workers
{
    public class ClienteCadastradoConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ClienteCadastradoConsumer(IOptions<RabbitMQSettings> options, IServiceScopeFactory serviceScopeFactory)
        {
            _factory = new ConnectionFactory
            {
                HostName = options.Value.HostName
            };
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = await _factory.CreateConnectionAsync(stoppingToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.BasicQosAsync(0, 1, false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                try
                {
                    var evento = JsonConvert.DeserializeObject<ClienteCadastradoEvent>(message);

                    if (evento is null)
                    {
                        await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                        return;
                    }

                    using var scope =  _serviceScopeFactory.CreateScope();
                    var propostaService = scope.ServiceProvider.GetRequiredService<IPropostaService>();

                    var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        //TODO: Logar o erro
                    });
                    await retryPolicy.ExecuteAsync(() => propostaService.CadastrarAsync(evento));

                    await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception)
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await channel.BasicConsumeAsync(queue: "cliente.cadastrado.queue", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
