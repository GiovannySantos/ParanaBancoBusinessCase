
using CartaoCredito.Application.DTOs;
using CartaoCredito.Application.Results;

namespace CartaoCredito.Application.Interfaces
{
    public interface ICartaoCreditoService
    {
        Task<CartaoCreditoResult> CadastrarAsync(CartaoCreditoDto cartaoCreditoDto);
    }
}
