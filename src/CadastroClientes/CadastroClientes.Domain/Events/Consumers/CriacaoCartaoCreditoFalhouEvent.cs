namespace CadastroClientes.Domain.Events.Consumers
{
    public class CriacaoCartaoCreditoFalhouEvent
    {
        public Guid ClienteId { get; set; }
        public Guid PropostaId { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}
