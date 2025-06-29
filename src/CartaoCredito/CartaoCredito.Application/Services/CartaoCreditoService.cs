using CartaoCredito.Application.DTOs;
using CartaoCredito.Application.Interfaces;
using CartaoCredito.Application.Results;
using CartaoCredito.Domain.Interfaces;

namespace CartaoCredito.Application.Services
{
    public class CartaoCreditoService(ICartaoCreditoRepository cartaoCreditoRepository) : ICartaoCreditoService
    {

        private readonly ICartaoCreditoRepository _cartaoCreditoRepository = cartaoCreditoRepository;

        public async Task<CartaoCreditoResult> CadastrarAsync(CartaoCreditoDto cartaoCreditoDto)
        {
            var validacoes = ValidarCartaoCreditoDto(cartaoCreditoDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            var cartaoCredito = await _cartaoCreditoRepository.CadastrarAsync(new(cartaoCreditoDto.PropostaId, cartaoCreditoDto.ClienteId, cartaoCreditoDto.NomeImpresso, cartaoCreditoDto.Limite));

            //publicar evento de criação do cartão de crédito

            return new(true, new { cartaoCredito });
        }

        private List<string>? ValidarCartaoCreditoDto(CartaoCreditoDto clienteDto)
        {
            return [];
        }
    }
}
