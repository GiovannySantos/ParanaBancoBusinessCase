using CadastroClientes.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Infra.DbContexts
{
    public class CadastroClientesDbContext(DbContextOptions<CadastroClientesDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteCartao> ClienteCartoes { get; set; }
        public DbSet<EventoErro> EventosErros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CadastroClientesDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
