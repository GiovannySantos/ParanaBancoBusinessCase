using CadastroClientes.Application.Interfaces;
using CadastroClientes.Domain.Events.Consumers;
using CadastroClientes.Infra.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CadastroClientes.API.Workers
{
    public class CriacaoCartaoCreditoFalhouConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CriacaoCartaoCreditoFalhouConsumer(IOptions<RabbitMQSettings> options, IServiceScopeFactory serviceScopeFactory)
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
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var evento = JsonConvert.DeserializeObject<CriacaoCartaoCreditoFalhouEvent>(message);

                    if (evento is null)
                    {
                        await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                        return;
                    }

                    await TratarErroAsync(evento);
                    await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception)
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await channel.BasicConsumeAsync("cartao.credito.falhou.queue", autoAck: false, consumer, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(1000, stoppingToken);
        }

        private async Task TratarErroAsync(CriacaoCartaoCreditoFalhouEvent evento)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(2),
                    (ex, ts, attempt, ctx) =>
                    {
                        Console.WriteLine($"[FalhaProposta] Tentativa {attempt} falhou. Erro: {ex.Message}");
                    });

            using var scope = _serviceScopeFactory.CreateScope();
            var erroService = scope.ServiceProvider.GetRequiredService<IEventoErroService>();

            await retryPolicy.ExecuteAsync(() =>
                erroService.RegistrarErroAsync(nameof(CriacaoCartaoCreditoFalhouEvent), "cartao.credito.falhou.queue", JsonConvert.SerializeObject(evento), evento.Motivo)
            );
        }
    }
}
