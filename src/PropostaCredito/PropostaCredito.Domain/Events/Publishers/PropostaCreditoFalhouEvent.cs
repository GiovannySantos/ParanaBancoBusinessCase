namespace PropostaCredito.Domain.Events.Publishers
{
    public class PropostaCreditoFalhouEvent(Guid propostaId, Guid clientId, string motivo)
    {
        public Guid ClienteId { get; set; } = clientId;
        public Guid PropostaId { get; set; } = propostaId;
        public string Motivo { get; set; } = motivo;
        public DateTime DataFalha { get; set; } = DateTime.UtcNow;
    }
}
