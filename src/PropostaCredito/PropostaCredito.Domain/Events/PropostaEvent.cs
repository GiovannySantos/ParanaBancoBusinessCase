
namespace PropostaCredito.Domain.Events
{
    public class PropostaEvent(Guid id, Guid clienteId, decimal valorAprovado, bool aprovado, string motivoRejeicao)
    {
        public Guid Id { get; } = id;
        public Guid ClienteId { get; } = clienteId;
        public decimal ValorAprovado { get; } = valorAprovado;
        public bool Aprovado { get; } = aprovado;
        public string? MotivoRejeicao { get; } = motivoRejeicao;
    }
}
