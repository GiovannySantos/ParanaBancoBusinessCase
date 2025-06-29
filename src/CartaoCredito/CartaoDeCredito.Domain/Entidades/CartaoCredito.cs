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

        public CartaoCredito(Guid propostaId, Guid clienteId, string nomeImpresso, decimal limite)
        {
            Id = Guid.NewGuid();
            PropostaId = propostaId;
            ClienteId = clienteId;
            NomeImpresso = nomeImpresso;
            Limite = limite;
            DataCriacao = DateTime.UtcNow;

            Numero = GerarNumero();
            Validade = CalcularValidade();
            Cvv = GerarCvv();
        }

        private static string GerarNumero()
        {
            var random = new Random();
            return $"{random.Next(4000, 4999)} {random.Next(1000, 9999)} {random.Next(1000, 9999)} {random.Next(1000, 9999)}";
        }

        private static string GerarCvv()
        {
            var random = new Random();
            return random.Next(100, 999).ToString();
        }

        private static DateTime CalcularValidade()
        {
            return DateTime.UtcNow.AddYears(5);
        }
    }

}
