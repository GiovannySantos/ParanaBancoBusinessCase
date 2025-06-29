namespace CartaoCredito.Application.Results
{
    public class CartaoCreditoResult(bool sucesso, object mensagem)
    {
        public bool Sucesso { get; set; } = sucesso;

        public object Mensagem { get; set; } = mensagem;
    }
}