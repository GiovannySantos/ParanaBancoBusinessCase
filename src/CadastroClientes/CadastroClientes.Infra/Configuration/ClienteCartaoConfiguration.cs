using CadastroClientes.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroClientes.Infra.Configuration
{
    public class ClienteCartaoConfiguration : IEntityTypeConfiguration<ClienteCartao>
    {
        public void Configure(EntityTypeBuilder<ClienteCartao> builder)
        {
            builder.ToTable("ClienteCartao");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.CartaoId).IsUnique(); // Evita duplicidade
            builder.HasIndex(x => x.ClienteId);
            builder.HasIndex(x => x.PropostaId);

            builder.Property(x => x.NumeroCartao).HasMaxLength(20).IsRequired();
            builder.Property(x => x.NomeImpresso).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Validade)
                .HasMaxLength(7) // Ex: "07-2027"
                .IsRequired();

            builder.Property(x => x.Limite)
                .HasColumnType("decimal(18,2)") // ou ajuste conforme sua precisão
                .IsRequired();
        }
    }
}
