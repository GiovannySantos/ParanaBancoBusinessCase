using CadastroClientes.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroClientes.Infra.Configuration
{
    public class EventoErroConfiguration : IEntityTypeConfiguration<EventoErro>
    {
        public void Configure(EntityTypeBuilder<EventoErro> builder)
        {
            builder.ToTable("EventoErro");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Origem).IsRequired().HasMaxLength(100);
            builder.Property(e => e.RoutingKey).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Payload).IsRequired();
            builder.Property(e => e.Erro).IsRequired();
        }
    }
}
