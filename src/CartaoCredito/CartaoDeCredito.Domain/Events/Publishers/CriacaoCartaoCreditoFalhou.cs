namespace CartaoCredito.Domain.Events.Publishers
{
    public class CriacaoCartaoCreditoFalhouEvent(Guid clienteId, Guid propostaId, string motivo)
    {
        public Guid ClienteId { get; set; } = clienteId;
        public Guid PropostaId { get; set; } = propostaId;
        public string Motivo { get; set; } = motivo;
        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}
