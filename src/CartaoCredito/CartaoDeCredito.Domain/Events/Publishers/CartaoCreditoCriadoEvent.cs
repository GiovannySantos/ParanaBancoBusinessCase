namespace CartaoCredito.Domain.Events.Publishers
{
    public class CartaoCreditoCriadoEvent(Guid cartaoId, Guid propostaId, Guid clienteId, string numeroCartao, string nomeImpresso, DateTime Validade, decimal limite)
    {
        public Guid CartaoId { get; set; } = cartaoId;
        public Guid ClienteId { get; set; } = clienteId;
        public Guid PropostaId { get; set; } = propostaId;
        public string NumeroCartao { get; set; } = numeroCartao;
        public string NomeImpresso { get; set; } = nomeImpresso;
        public DateTime Validade { get; set; } = Validade;
        public decimal Limite { get; set; } = limite;
    }

}
