using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Application.Results;
using PropostaCredito.Domain.Entidades;
using PropostaCredito.Domain.Events.Consumers;
using PropostaCredito.Domain.Events.Publishers;
using PropostaCredito.Domain.Interfaces;

namespace PropostaCredito.Application.Services
{
    public class PropostaService(IPropostaRepository propostaRepository, IEventPublisher eventPublisher) : IPropostaService
    {

        private readonly IPropostaRepository _propostaRepository = propostaRepository;
        private readonly IEventPublisher _publisher = eventPublisher;

        public async Task<PropostaResult> InserirAsync(PropostaDto propostaDto)
        {
            var validacoes = Validar(propostaDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            Proposta? proposta = await _propostaRepository.CadastrarAsync(new(propostaDto.ClienteId, propostaDto.ValorSolicitado, propostaDto.RendaMensal));

            return new(true, proposta);
        }

        public async Task<PropostaResult> CadastrarAsync(ClienteCadastradoEvent evento)
        {
            var validacoes = Validar(evento);
            if (validacoes != null && validacoes.Count > 0)
            {
                return new(false, validacoes);
            }

            Proposta proposta = new(evento.ClienteId, evento.ValorCreditoDesejado, evento.RendaMensal);
            await _propostaRepository.CadastrarAsync(proposta);

            //Notificar os serviços que a proposta foi criada (aprovada ou não)
            if (proposta.Aprovada)
            {
                // Publica o evento de proposta aprovada para o serviço de cartão de crédito
                await PublishPropostaAprovada(proposta, evento);
                return new(true, proposta);
            }
            else
            {
                var propostaCreditoFalhou = new PropostaCreditoFalhouEvent(proposta.Id, proposta.ClienteId, proposta.MotivoRejeicao);
                await _publisher.PublishAsync(propostaCreditoFalhou, new("proposta.credito.events", "direct" ,"proposta.credito.falhou"));
                return new(false, proposta);
            }
        }

        private async Task PublishPropostaAprovada(Proposta proposta, ClienteCadastradoEvent evento)
        {
            var propostaCriadaEvent = new PropostaEvent(
                proposta.Id,
                proposta.ClienteId,
                evento.Nome,
                proposta.ValorSolicitado,
                proposta.Aprovada,
                proposta.MotivoRejeicao ?? string.Empty
            );
            await _publisher.PublishAsync(propostaCriadaEvent, new("proposta.credito.events", "direct", "proposta.aprovada"));
        }

        public Task<PropostaResult> ObterPropostaAsync(Guid idProposta)
        {
            throw new NotImplementedException();
        }

        //Valida somente o básico, as validações de negócio estão na entidade Proposta
        private static List<string>? Validar(object proposta)
        {
            List<string> erros = [];

            if (((ClienteCadastradoEvent)proposta).RendaMensal <= 0)
                erros.Add("Renda mensal deve ser maior que zero.");

            if (((ClienteCadastradoEvent)proposta).ValorCreditoDesejado <= 0)
                erros.Add("Valor solicitado deve ser maior que zero.");

            if (((ClienteCadastradoEvent)proposta).ClienteId == Guid.Empty)
                erros.Add("ClienteId é obrigatório.");

            return erros.Count > 0 ? erros : null;
        }
    }
}