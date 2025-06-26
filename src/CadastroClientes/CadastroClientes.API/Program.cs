using CadastroClientes.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

//Adicionando SQLite como provedor de banco de dados
var dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "clientes.db");
builder.Services.AddDbContext<CadastroClientesDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

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
