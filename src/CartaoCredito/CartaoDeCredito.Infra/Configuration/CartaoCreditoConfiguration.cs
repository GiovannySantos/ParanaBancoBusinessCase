using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartaoCredito.Infra.Configuration
{
    public class CartaoCreditoConfiguration : IEntityTypeConfiguration<CartaoDeCredito.Domain.Entidades.CartaoCredito>
    {
        public void Configure(EntityTypeBuilder<CartaoDeCredito.Domain.Entidades.CartaoCredito> builder)
        {
            builder.ToTable("CartoesCredito");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.PropostaId)
                .IsRequired();

            builder.Property(c => c.ClienteId)
                .IsRequired();

            builder.Property(c => c.Numero)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(c => c.NomeImpresso)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Validade)
                .IsRequired();

            builder.Property(c => c.Cvv)
                .IsRequired()
                .HasMaxLength(4);

            builder.Property(c => c.Limite)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.DataExpiracao)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
