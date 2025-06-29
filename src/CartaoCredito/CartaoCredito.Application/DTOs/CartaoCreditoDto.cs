namespace CartaoCredito.Application.DTOs
{
    public class CartaoCreditoDto(Guid propostaId, Guid clienteId, decimal limite, string nomeImpresso)
    {
        public Guid PropostaId { get; set; } = propostaId;
        public Guid ClienteId { get; set; } = clienteId;
        public decimal Limite { get; set; } = limite;
        public string NomeImpresso { get; set; } = nomeImpresso;
    }
}