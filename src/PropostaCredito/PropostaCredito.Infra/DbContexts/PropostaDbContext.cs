using Microsoft.EntityFrameworkCore;
using PropostaCredito.Domain.Entidades;

namespace PropostaCredito.Infra.DbContexts
{
    public class PropostaDbContext : DbContext
    {
        public PropostaDbContext(DbContextOptions<PropostaDbContext> options) : base(options)
        {
        }

        public DbSet<Proposta> Propostas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropostaDbContext).Assembly);
        }
    }
}
