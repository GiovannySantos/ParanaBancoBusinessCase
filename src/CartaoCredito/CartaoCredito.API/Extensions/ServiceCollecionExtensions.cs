using CartaoCredito.Application.Interfaces;
using CartaoCredito.Application.Services;
using CartaoCredito.Domain.Interfaces;
using CartaoCredito.Infra.DbContexts;
using CartaoCredito.Infra.Messaging;
using CartaoCredito.Infra.Repositories;
using CartaoCredito.Infra.Settings;
using Microsoft.EntityFrameworkCore;

namespace CartaoCredito.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;
        bool useSqlite = config.GetValue<bool>("UseSqlite");

        services.AddDbContext<CartaoCreditoDbContext>(options =>
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
        services.AddScoped<ICartaoCreditoService, CartaoCreditoService>();

        // Domain/Infra Layer
        services.AddScoped<ICartaoCreditoRepository, CartaoCreditoRepository>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
