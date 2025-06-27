namespace CadastroClientes.Application.Results
{
    public class ClientesResult
    {
        public bool Sucesso { get; set; }

        public object Mensagem { get; set; }

        public ClientesResult(bool sucesso, object mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }

    }
}
