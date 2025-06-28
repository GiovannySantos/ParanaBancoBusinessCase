using CadastroClientes.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroClientes.Infra.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(c => c.DataNascimento)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(150);

            builder.Property(c => c.Telefone)
                .HasMaxLength(15);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            builder.Property(c => c.RendaMensal)
                .IsRequired();

            builder.Property(c => c.ValorCreditoDesejado)
                .IsRequired();
        }
    }
}
