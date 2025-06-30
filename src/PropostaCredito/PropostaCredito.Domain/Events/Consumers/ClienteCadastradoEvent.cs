namespace PropostaCredito.Domain.Events.Consumers
{
    public class ClienteCadastradoEvent
    {
        public Guid ClienteId { get; set; }
        public decimal RendaMensal { get; set; }
        public decimal ValorCreditoDesejado { get; set; }
    }
}
