using CartaoCredito.Application.DTOs;
using CartaoCredito.Application.Interfaces;
using CartaoCredito.Application.Results;
using CartaoCredito.Domain.Events.Consumers;
using CartaoCredito.Domain.Events.Publishers;
using CartaoCredito.Domain.Interfaces;

namespace CartaoCredito.Application.Services
{
    public class CartaoCreditoService(ICartaoCreditoRepository cartaoCreditoRepository, IEventPublisher eventPublisher) : ICartaoCreditoService
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository = cartaoCreditoRepository;
        private readonly IEventPublisher _publisher = eventPublisher;

        public async Task<CartaoCreditoResult> CadastrarPorPropostaAsync(PropostaEvent propostaEvent)
        {
            var validacoes = Validar(propostaEvent);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            try
            {
                var cartaoCredito = await _cartaoCreditoRepository.CadastrarAsync(
                    new(propostaEvent.Id, propostaEvent.ClienteId, propostaEvent.NomeCliente, propostaEvent.ValorAprovado));

                await PublicarCartaoAprovado(cartaoCredito);

                return new(true, new { cartaoCredito });
            }
            catch (Exception ex)
            {
                // Aqui você publica o evento de falha
                var falhaEvent = new CriacaoCartaoCreditoFalhouEvent(propostaEvent.Id,propostaEvent.ClienteId,$"Erro ao gerar cartão: {ex.Message}");

                await _publisher.PublishAsync(falhaEvent,new("cartao.credito.events","direct", "cartao.credito.falhou"));

                // Você pode optar por retornar false ou relançar a exceção
                return new(false, "Erro ao gerar cartão de crédito.");
            }
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

        public async Task<CartaoCreditoResult> CadastrarAsync(CartaoCreditoDto cartaoCreditoDto)
        {
            var propostaEvent = new PropostaEvent() 
            { 
                Id = cartaoCreditoDto.PropostaId,
                ClienteId = cartaoCreditoDto.ClienteId,
                NomeCliente = cartaoCreditoDto.NomeImpresso,
                ValorAprovado = cartaoCreditoDto.Limite,
                Aprovado = true,
                MotivoRejeicao = string.Empty
            };

            var validacoes = Validar(propostaEvent);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            Domain.Entidades.CartaoCredito cartaoCredito = await _cartaoCreditoRepository.CadastrarAsync(new(propostaEvent.Id, propostaEvent.ClienteId, propostaEvent.NomeCliente, propostaEvent.ValorAprovado));

            return new(true, new { cartaoCredito });
        }

        private static List<string>? Validar(PropostaEvent propostaEvent)
        {
            List<string> erros = [];

            if (propostaEvent.Id == Guid.Empty)
                erros.Add("PropostaId é obrigatório.");

            if (propostaEvent.ClienteId == Guid.Empty)
                erros.Add("ClienteId é obrigatório.");

            if (string.IsNullOrWhiteSpace(propostaEvent.NomeCliente))
                erros.Add("Nome impresso no cartão é obrigatório.");

            if (propostaEvent.ValorAprovado <= 0)
                erros.Add("Valor deve ser maior que zero.");
            return erros.Count > 0 ? erros : null;
        }
    }
}
