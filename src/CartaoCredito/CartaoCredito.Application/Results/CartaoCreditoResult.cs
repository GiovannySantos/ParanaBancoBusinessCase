namespace CartaoCredito.Application.Results
{
    public class CartaoCreditoResult
    {
        public bool Sucesso { get; set; }

        public object Mensagem { get; set; }

        public CartaoCreditoResult(bool sucesso, object mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }
}