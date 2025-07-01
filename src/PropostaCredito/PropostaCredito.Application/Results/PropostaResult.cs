namespace PropostaCredito.Application.Results
{
    public class PropostaResult(bool sucesso, object mensagem, object? proposta)
    {
        public bool Sucesso { get; private set; } = sucesso;

        public object Mensagem { get; private set; } = mensagem;

        public object? PropostaRetorno { get; private set; } = proposta;
    }
}
