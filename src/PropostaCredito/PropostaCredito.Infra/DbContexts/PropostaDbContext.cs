using Microsoft.EntityFrameworkCore;
using PropostaCredito.Domain.Entidades;

namespace PropostaCredito.Infra.DbContexts
{
    public class PropostaDbContext(DbContextOptions<PropostaDbContext> options) : DbContext(options)
    {
        public DbSet<Proposta> Propostas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropostaDbContext).Assembly);
        }
    }
}
