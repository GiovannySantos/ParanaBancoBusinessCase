namespace PropostaCredito.Application.Results
{
    public class PropostaResult
    {
        public bool Sucesso { get; set; }

        public object Mensagem { get; set; }

        public PropostaResult(bool sucesso, object mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }
}
