namespace PropostaCredito.Domain.Entidades
{
    public class Proposta
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal ValorSolicitado { get; set; }
        public decimal RendaMensal { get; set; }
        public DateTime? DataAprovacao { get; private set; }
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public bool Aprovada { get; private set; } = false;
        public string? MotivoRejeicao { get; private set; }

        protected Proposta() { } // EF Core

        public Proposta(Guid clienteId, decimal valorSolicitado, decimal rendaMensal)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            ValorSolicitado = valorSolicitado;
            RendaMensal = rendaMensal;

            // Avalia a proposta no momento da criação
            Avaliar();
        }

        private bool Avaliar()
        {
            // Lógica de avaliação da proposta
            // Exemplo: se o valor solicitado for maior que 3 vezes a renda mensal, rejeita
            if (ValorSolicitado > RendaMensal * 3)
            {
                Rejeitar("Valor solicitado excede o limite permitido.");
                return false;
            }
            // Se passar na avaliação, aprova a proposta
            Aprovar();
            return true;
        }

        private void Rejeitar(string motivo)
        {
            Aprovada = false;
            MotivoRejeicao = motivo;
            DataAprovacao = DateTime.UtcNow;
        }

        private void Aprovar()
        {
            Aprovada = true;
            DataAprovacao = DateTime.UtcNow;
        }
    }
}
