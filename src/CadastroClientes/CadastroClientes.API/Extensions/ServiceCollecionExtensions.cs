using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Services;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using CadastroClientes.Infra.Messaging;
using CadastroClientes.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;
        var useSqlite = config.GetValue<bool>("UseSqlite");

        services.AddDbContext<CadastroClientesDbContext>(options =>
        {
            if (useSqlite)
                options.UseSqlite(config.GetConnectionString("SQLite"));
            else
                options.UseNpgsql(config.GetConnectionString("Postgres") ??
                    Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection"));
        });

        // Messaging Layer
        builder.Services.AddSingleton<IEventPublisher, RabbitMQPublisher>(provider => new RabbitMQPublisher("rabbitmq"));

        // Application Layer
        services.AddScoped<IClienteService, ClienteService>();

        // Domain/Infra Layer
        services.AddScoped<IClienteRepository, ClienteRepository>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
