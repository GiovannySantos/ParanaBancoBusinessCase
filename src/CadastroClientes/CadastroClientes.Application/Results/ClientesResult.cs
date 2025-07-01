using CadastroClientes.Application.DTOs;

namespace CadastroClientes.Application.Results
{
    public class ClientesResult(bool sucesso, object mensagem, object cliente)
    {
        public bool Sucesso { get; private set; } = sucesso;

        public object Mensagem { get; private set; } = mensagem;

        public object ClienteRetorno { get; private set; } = cliente;
    }
}
