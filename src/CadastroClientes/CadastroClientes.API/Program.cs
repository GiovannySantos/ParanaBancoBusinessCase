using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Services;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using CadastroClientes.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Adicionando SQLite como provedor de banco de dados
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection");


builder.Services.AddDbContext<CadastroClientesDbContext>(options => options.UseNpgsql(connectionString));


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
    db.Database.Migrate();
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

app.Run();
