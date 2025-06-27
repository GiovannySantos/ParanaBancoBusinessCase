using CadastroClientes.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Infra.Data
{
    public class CadastroClientesDbContext : DbContext
    {
        public CadastroClientesDbContext(DbContextOptions<CadastroClientesDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CadastroClientesDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
