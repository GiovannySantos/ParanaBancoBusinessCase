﻿using CartaoCredito.Infra.DbContexts;
using CartaoCredito.Infra.Messaging;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;

namespace CartaoCredito.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }

    public static void MigrateDatabaseWithRetry(this WebApplication app)
    {
        var retryPolicy = Policy
            .Handle<NpgsqlException>()
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(2),
                (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"[DB Retry] Tentativa {retryCount} falhou. Aguardando {timeSpan.TotalSeconds}s...");
                });

        retryPolicy.Execute(() =>
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CartaoCreditoDbContext>();
            db.Database.Migrate();
        });
    }

    public static async Task InitializeMessagingAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<RabbitMQInitializer>();
        await initializer.InitializeAsync();
    }
}
