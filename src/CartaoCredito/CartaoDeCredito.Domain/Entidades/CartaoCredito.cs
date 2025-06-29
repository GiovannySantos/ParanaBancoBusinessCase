namespace CartaoCredito.Domain.Entidades
{
    public class CartaoCredito
    {
        public Guid Id { get; private set; }
        public Guid PropostaId { get; private set; }
        public Guid ClienteId { get; private set; }
        public string Numero { get; private set; }
        public string NomeImpresso { get; private set; }
        public DateTime Validade { get; private set; }
        public string Cvv { get; private set; }
        public decimal Limite { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataExpiracao { get; private set; }

        private CartaoCredito() { }

        //Serve apenas para gerar os dados de uma forma randomica
        private static readonly Random _random = new();

        public CartaoCredito(Guid propostaId, Guid clienteId, string nomeImpresso, decimal limite)
        {
            Id = Guid.NewGuid();
            PropostaId = propostaId;
            ClienteId = clienteId;
            NomeImpresso = nomeImpresso.ToUpperInvariant();
            Limite = limite;
            DataCriacao = DateTime.UtcNow;

            Numero = GerarNumero();
            Validade = CalcularValidade();
            Cvv = GerarCvv();
        }

        private static string GerarNumero()
        {
            return $"{_random.Next(4000, 4999)} {_random.Next(1000, 9999)} {_random.Next(1000, 9999)} {_random.Next(1000, 9999)}";
        }

        private static string GerarCvv()
        {
            return _random.Next(100, 999).ToString();
        }

        private static DateTime CalcularValidade()
        {
            return DateTime.UtcNow.AddYears(5);
        }
    }

}
