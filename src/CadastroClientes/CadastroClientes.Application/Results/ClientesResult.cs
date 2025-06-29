namespace CadastroClientes.Application.Results
{
    public class ClientesResult(bool sucesso, object mensagem)
    {
        public bool Sucesso { get; private set; } = sucesso;

        public object Mensagem { get; private set; } = mensagem;
    }
}
