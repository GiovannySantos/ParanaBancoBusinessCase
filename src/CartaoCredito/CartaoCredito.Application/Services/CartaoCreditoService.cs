using CartaoCredito.Application.DTOs;
using CartaoCredito.Application.Interfaces;
using CartaoCredito.Application.Results;
using CartaoCredito.Domain.Events;
using CartaoCredito.Domain.Interfaces;

namespace CartaoCredito.Application.Services
{
    public class CartaoCreditoService(ICartaoCreditoRepository cartaoCreditoRepository, IEventPublisher eventPublisher) : ICartaoCreditoService
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository = cartaoCreditoRepository;
        private readonly IEventPublisher _publisher = eventPublisher;

        public async Task<CartaoCreditoResult> CadastrarAsync(CartaoCreditoDto cartaoCreditoDto)
        {
            var validacoes = ValidarCartaoCreditoDto(cartaoCreditoDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            Domain.Entidades.CartaoCredito cartaoCredito = await _cartaoCreditoRepository.CadastrarAsync(new(cartaoCreditoDto.PropostaId, cartaoCreditoDto.ClienteId, cartaoCreditoDto.NomeImpresso, cartaoCreditoDto.Limite));
            
            await PublicarCartaoAprovado(cartaoCredito);

            return new(true, new { cartaoCredito });
        }

        private async Task PublicarCartaoAprovado(Domain.Entidades.CartaoCredito cartaoCredito)
        {
            //publicar evento de criação do cartão de crédito
            CartaoCreditoCriadoEvent cartaoCreditoCriadoEvent = new(
                cartaoCredito.Id,
                cartaoCredito.PropostaId,
                cartaoCredito.ClienteId,
                cartaoCredito.Numero,
                cartaoCredito.NomeImpresso,
                cartaoCredito.Validade,
                cartaoCredito.Limite
            );
            await _publisher.PublishAsync(cartaoCreditoCriadoEvent, new("cartao.credito.events", "direct", "cartao.credito.criado"));
        }

        private static List<string>? ValidarCartaoCreditoDto(CartaoCreditoDto clienteDto)
        {
            List<string> erros = [];

            if (clienteDto.PropostaId == Guid.Empty)
                erros.Add("PropostaId é obrigatório.");

            if (clienteDto.ClienteId == Guid.Empty)
                erros.Add("ClienteId é obrigatório.");

            if (string.IsNullOrWhiteSpace(clienteDto.NomeImpresso))
                erros.Add("Nome impresso no cartão é obrigatório.");

            if (clienteDto.Limite <= 0)
                erros.Add("Limite do cartão deve ser maior que zero.");
            return erros.Count > 0 ? erros : null;
        }
    }
}
