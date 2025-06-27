using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Services;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.Data;
using CadastroClientes.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Adicionando SQLite como provedor de banco de dados
var dbPath = Path.GetFullPath(Path.Combine("..", "clientes.db"));
builder.Services.AddDbContext<CadastroClientesDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

// Registrando os repositórios e serviços
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
