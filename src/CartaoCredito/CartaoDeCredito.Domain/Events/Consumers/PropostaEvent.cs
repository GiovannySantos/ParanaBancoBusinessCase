namespace CartaoCredito.Domain.Events.Consumers
{
    public class PropostaEvent
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string NomeCliente { get; set; }
        public decimal ValorAprovado { get; set; }
        public bool Aprovado { get; set; }
        public string? MotivoRejeicao { get; set; }
    }
}
