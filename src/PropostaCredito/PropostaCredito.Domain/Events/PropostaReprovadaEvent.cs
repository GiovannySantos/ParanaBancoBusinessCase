

namespace PropostaCredito.Domain.Events
{
    public class PropostaReprovadaEvent(Guid id, Guid clienteId, decimal valorSolicitado, string? motivoRejeicao)
    {
        public Guid Id { get; } = id;
        public Guid ClienteId { get; } = clienteId;
        public decimal ValorSolicitado { get; } = valorSolicitado;
        public string? MotivoRejeicao { get; } = motivoRejeicao;
    }
}
