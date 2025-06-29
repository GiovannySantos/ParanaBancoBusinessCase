using System.ComponentModel.DataAnnotations;

namespace CartaoCredito.Application
{
    public class CartaoCreditoDto
    {
        public Guid PropostaId { get; set; }
        public Guid ClienteId { get; set; }
        public decimal Limite { get; set; }
        public string NomeImpresso { get; set; }
    }
}