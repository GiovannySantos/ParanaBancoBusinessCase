using CadastroClientes.API.Workers;
using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Services;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using CadastroClientes.Infra.Messaging;
using CadastroClientes.Infra.Repositories;
using CadastroClientes.Infra.Settings;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;
        bool useSqlite = config.GetValue<bool>("UseSqlite");

        services.AddDbContext<CadastroClientesDbContext>(options =>
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
        services.AddScoped<IClienteService, ClienteService>();

        // Domain/Infra Layer
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClienteCartaoRepository, ClienteCartaoRepository>();

        //Background Services
        builder.Services.AddHostedService<CartaoCreditoCriadoConsumer>();


        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
