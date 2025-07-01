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

        public Proposta(Guid clienteId, decimal valorSolicitado, decimal rendaMensal, bool aprovada, string motivoRejeicao)
        {
            ClienteId = clienteId;
            ValorSolicitado = valorSolicitado;
            RendaMensal = rendaMensal;

            Rejeitar(motivoRejeicao);
        }

        private void Avaliar()
        {
            // Lógica de avaliação da proposta
            //Se o valor solicitado for maior que 3 vezes a renda mensal, rejeita
            if (ValorSolicitado > RendaMensal * 3)
            {
                Rejeitar("Valor solicitado excede o limite permitido.");
                return;
            }

            //Se o valor solicitado for = 0, rejeita
            if (ValorSolicitado == 0)
            {
                Rejeitar("Cliente já atingiu o limite máximo de crédito permitido.");
                return;
            }

            //Se a renda mensal for menor que 1000, rejeita
            if (RendaMensal < 1000)
            {
                Rejeitar("Renda mensal insuficiente para aprovação.");
                return;
            }

            // Se passar na avaliação, aprova a proposta
            Aprovar();
            return;
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
