using Microsoft.EntityFrameworkCore;
using PropostaCredito.API.Workers;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Application.Services;
using PropostaCredito.Domain.Interfaces;
using PropostaCredito.Infra.DbContexts;
using PropostaCredito.Infra.Messaging;
using PropostaCredito.Infra.Repositories;
using PropostaCredito.Infra.Settings;

namespace PropostaCredito.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var config = builder.Configuration;
            bool useSqlite = config.GetValue<bool>("UseSqlite");

            services.AddDbContext<PropostaDbContext>(options =>
            {
                if (useSqlite)
                    options.UseSqlite(config.GetConnectionString("SQLite"));
                else
                    options.UseNpgsql(config.GetConnectionString("Postgres") ?? Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection"));
            });

            // Messaging Layer
            builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

            builder.Services.AddSingleton<RabbitMQInitializer>();
            builder.Services.AddSingleton<IEventPublisher, RabbitMQPublisher>();

            // Application Layer
            services.AddScoped<IPropostaService, PropostaService>();

            // Domain/Infra Layer
            services.AddScoped<IPropostaRepository, PropostaRepository>();

            //Background Services
            builder.Services.AddHostedService<ClienteCadastradoConsumer>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
