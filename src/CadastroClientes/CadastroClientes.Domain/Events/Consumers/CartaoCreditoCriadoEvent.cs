namespace CadastroClientes.Domain.Events.Consumers
{
    public class CartaoCreditoCriadoEvent
    {
        public Guid CartaoId { get; set; }
        public Guid ClienteId { get; set; }
        public Guid PropostaId { get; set; }
        public string NumeroCartao { get; set; }
        public string NomeImpresso { get; set; }
        public DateTime Validade { get; set; }
        public decimal Limite { get; set; }
    }
}
