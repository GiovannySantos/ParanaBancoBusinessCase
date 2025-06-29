namespace PropostaCredito.Application.DTOs
{
    public class PropostaDto
    {
        public Guid ClienteId { get; set; }
        public decimal RendaMensal { get; set; }
        public decimal ValorSolicitado { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public PropostaDto(Guid clienteId, decimal valorSolicitado, decimal rendaMensal)
        {
            ClienteId = clienteId;
            ValorSolicitado = valorSolicitado;
            RendaMensal = rendaMensal;
        }
    }
}
