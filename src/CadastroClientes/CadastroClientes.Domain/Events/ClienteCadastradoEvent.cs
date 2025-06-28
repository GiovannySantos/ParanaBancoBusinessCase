namespace CadastroClientes.Domain.Events
{
    public class ClienteCadastradoEvent(Guid clienteId, string nome, string cpf, DateTime dataNascimento, string email, string telefone, decimal rendaMensal, decimal valorCreditoDesejado)
    {
        public Guid ClienteId { get; } = clienteId;
        public string Nome { get; } = nome;
        public string Cpf { get; } = cpf;
        public DateTime DataNascimento { get; } = dataNascimento;
        public string Email { get; } = email;
        public string Telefone { get; } = telefone;
        public decimal RendaMensal { get; } = rendaMensal;
        public decimal ValorCreditoDesejado { get; } = valorCreditoDesejado;
    }
}