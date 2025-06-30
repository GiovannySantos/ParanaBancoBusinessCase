
using CartaoCredito.Application.DTOs;
using CartaoCredito.Application.Results;
using CartaoCredito.Domain.Events.Consumers;

namespace CartaoCredito.Application.Interfaces
{
    public interface ICartaoCreditoService
    {
        Task<CartaoCreditoResult> CadastrarPorPropostaAsync(PropostaEvent propostaEvent);
        Task<CartaoCreditoResult?> CadastrarAsync(CartaoCreditoDto cartaoCreditoDto);
    }
}
