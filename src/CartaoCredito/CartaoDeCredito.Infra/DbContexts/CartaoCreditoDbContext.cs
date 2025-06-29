using CartaoDeCredito.Domain.Entidades;
using CartaoDeCredito.Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CartaoDeCredito.Infra.DbContexts
{
    public class CartaoCreditoDbContext : DbContext
    {
        public CartaoCreditoDbContext(DbContextOptions<CartaoCreditoDbContext> options) : base(options)
        {
        }

        public DbSet<CartaoCredito> CartoesCredito { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartaoCreditoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
