namespace CadastroClientes.Domain.Entidades
{
    public class Cliente
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        //Dados pessoais
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }

        //Dados financeiros do cliente
        public decimal RendaMensal { get; set; }
        public decimal ValorCreditoDesejado { get; set; }

        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;

        protected Cliente() { } // EF Core

        public Cliente(string nome, string cpf, DateTime dataNascimento, string email, string telefone, decimal rendaMensal, decimal valorCreditoDesejado)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento.Date.ToUniversalTime();
            Email = email;
            Telefone = telefone;
            RendaMensal = rendaMensal;
            ValorCreditoDesejado = valorCreditoDesejado;
        }
    }
}
