namespace CartaoCredito.Domain.Interfaces
{
    public interface ICartaoCreditoRepository
    {
        Task<Entidades.CartaoCredito> CadastrarAsync(Entidades.CartaoCredito cartaoCredito);
    }
}
