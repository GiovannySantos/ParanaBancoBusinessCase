using CartaoCredito.Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CartaoCredito.Infra.DbContexts
{
    public class CartaoCreditoDbContext : DbContext
    {
        public CartaoCreditoDbContext(DbContextOptions<CartaoCreditoDbContext> options) : base(options)
        {
        }

        public DbSet<CartaoDeCredito.Domain.Entidades.CartaoCredito> CartoesCredito { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartaoCreditoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
