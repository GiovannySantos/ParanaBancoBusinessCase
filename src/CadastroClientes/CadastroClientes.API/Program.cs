using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Services;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using CadastroClientes.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var useSqlite = config.GetValue<bool>("UseSqlite");

builder.Services.AddDbContext<CadastroClientesDbContext>(options =>
{
    if (useSqlite)
        options.UseSqlite(config.GetConnectionString("SQLite"));
    else
    {
        options.UseNpgsql(config.GetConnectionString("Postgres") ?? Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection"));
    }
});

// Registrando os repositórios e serviços
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CadastroClientesDbContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
