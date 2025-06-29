using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropostaCredito.Domain.Entidades;

namespace PropostaCredito.Infra.Configuration
{
    public class PropostaConfiguration : IEntityTypeConfiguration<Proposta>
    {
        public void Configure(EntityTypeBuilder<Proposta> builder)
        {
            builder.ToTable("Propostas");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClienteId)
                .IsRequired();

            builder.Property(p => p.ValorSolicitado)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.RendaMensal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DataAprovacao)
                .IsRequired(false);

            builder.Property(p => p.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(p => p.Aprovada)
                .IsRequired();

            builder.Property(p => p.MotivoRejeicao)
                .HasMaxLength(500);
        }
    }
}
