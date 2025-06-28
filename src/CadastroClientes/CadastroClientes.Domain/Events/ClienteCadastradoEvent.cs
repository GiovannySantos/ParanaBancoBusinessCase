namespace CadastroClientes.Domain.Events
{
    public class ClienteCadastradoEvent
    {
        public Guid ClienteId { get; }
        public string Nome { get; }
        public string Cpf { get; }
        public DateTime DataNascimento { get; }
        public string Email { get; }
        public string Telefone { get; }

        public ClienteCadastradoEvent(Guid clienteId, string nome, string cpf, DateTime dataNascimento, string email, string telefone)
        {
            ClienteId = clienteId;
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
        }
    }
}