namespace CadastroClientes.Domain.Entidades
{
    public class ClienteCartao
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid CartaoId { get; private set; }
        public Guid PropostaId { get; private set; }

        public string NumeroCartao { get; private set; } = null!;
        public string NomeImpresso { get; private set; } = null!;
        public string Validade { get; private set; }
        public decimal Limite { get; private set; }

        protected ClienteCartao() { } // EF Core

        public ClienteCartao(Guid clienteId, Guid cartaoId,Guid propostaId, string numeroCartao, string nomeImpresso, DateTime validade, decimal limite)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            PropostaId = propostaId;
            CartaoId = cartaoId;
            NumeroCartao = numeroCartao;
            NomeImpresso = nomeImpresso;
            Validade = validade.ToString("MM-yyyy");
            Limite = limite;
        }
    }
}
