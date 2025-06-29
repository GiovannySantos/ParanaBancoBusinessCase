using CartaoCredito.Application.Interfaces;
using CartaoCredito.Application.Results;
using CartaoDeCredito.Domain.Interfaces;

namespace CartaoCredito.Application.Services
{
    public class CartaoCreditoService : ICartaoCreditoService
    {

        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public CartaoCreditoService(ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

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
