
namespace PropostaCredito.Domain.Events
{
    public class PropostaAprovadaEvent(Guid id, Guid clienteId, decimal ValorAprovado)
    {
        public Guid Id { get; } = id;
        public Guid ClienteId { get; }  = clienteId;
        public decimal ValorAprovado { get; } = ValorAprovado;
    }
}
