using CartaoDeCredito.Domain.Entidades;

namespace CartaoDeCredito.Domain.Interfaces
{
    public interface ICartaoCreditoRepository
    {
        Task<CartaoCredito> CadastrarAsync(CartaoCredito cartaoCredito);
    }
}
