using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Application.Results;
using PropostaCredito.Domain.Entidades;
using PropostaCredito.Domain.Events;
using PropostaCredito.Domain.Interfaces;

namespace PropostaCredito.Application.Services
{
    public class PropostaService(IPropostaRepository propostaRepository, IEventPublisher eventPublisher) : IPropostaService
    {

        private readonly IPropostaRepository _propostaRepository = propostaRepository;
        private readonly IEventPublisher _publisher = eventPublisher;

        public async Task<PropostaResult> CadastrarAsync(PropostaDto propostaDto)
        {
            var validacoes = ValidarPropostaDto(propostaDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            var proposta = await _propostaRepository.CadastrarAsync(new(propostaDto.ClienteId, propostaDto.ValorSolicitado, propostaDto.RendaMensal));

            //Notificar os serviços que a proposta foi criada (aprovada ou não)
            if (proposta.Aprovada)
            {
                // Publica o evento de proposta aprovada para o serviço de cartão de crédito
                await PublishPropostaAprovada(proposta);
            }
            else
            {
                // Publica o evento de proposta reprovada para o serviço de cadastro de clientes
                await PublishPropostaReprovada(proposta);
                return new(true, proposta);
            }

            return new(true, proposta);

        }

        private async Task PublishPropostaAprovada(Proposta proposta)
        {
            var propostaCriadaEvent = new PropostaAprovadaEvent(
                proposta.Id,
                proposta.ClienteId,
                proposta.ValorSolicitado
            );
            await _publisher.PublishAsync(propostaCriadaEvent, new("proposta.credito.events", "direct", "proposta.aprovada"));
        }

        private async Task PublishPropostaReprovada(Proposta proposta)
        {
            var propostaReprovadaEvent = new PropostaReprovadaEvent(
                proposta.Id,
                proposta.ClienteId,
                proposta.ValorSolicitado,
                proposta.MotivoRejeicao
            );
            await _publisher.PublishAsync(propostaReprovadaEvent, new("proposta.credito.events", "direct", "proposta.reprovada"));
        }

        public Task<PropostaResult> ObterPropostaAsync(Guid idProposta)
        {
            throw new NotImplementedException();
        }

        private List<PropostaResult> ValidarPropostaDto(PropostaDto propostaDto)
        {
            return [];
        }
    }
}