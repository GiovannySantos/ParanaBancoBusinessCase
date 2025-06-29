namespace PropostaCredito.Application.DTOs
{
    public class PropostaDto(Guid clienteId, decimal valorSolicitado, decimal rendaMensal)
    {
        public Guid ClienteId { get; set; } = clienteId;
        public decimal RendaMensal { get; set; } = rendaMensal;
        public decimal ValorSolicitado { get; set; } = valorSolicitado;
    }
}
