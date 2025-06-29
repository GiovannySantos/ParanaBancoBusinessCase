using CartaoCredito.Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CartaoCredito.Infra.DbContexts
{
    public class CartaoCreditoDbContext(DbContextOptions<CartaoCreditoDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entidades.CartaoCredito> CartoesCredito { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartaoCreditoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
