namespace PropostaCredito.Application.Results
{
    public class PropostaResult(bool sucesso, object mensagem)
    {
        public bool Sucesso { get; set; } = sucesso;

        public object Mensagem { get; set; } = mensagem;
    }
}
