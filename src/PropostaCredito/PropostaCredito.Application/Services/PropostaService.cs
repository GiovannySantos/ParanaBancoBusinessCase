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

        //Método exposto para ser chamado pelo controller ou serviço externo (apenas para fins de teste do serviço separadamente)
        public async Task<PropostaResult> InserirAsync(PropostaDto propostaDto)
        {
            var validacoes = Validar(propostaDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes, validacoes);

            Proposta? proposta = await _propostaRepository.CadastrarAsync(new(propostaDto.ClienteId, propostaDto.ValorSolicitado, propostaDto.RendaMensal));

            return new(true, "Proposta inserida com sucesso", proposta);
        }

        public async Task<PropostaResult> CadastrarAsync(ClienteCadastradoEvent evento)
        {
            var validacoes = Validar(evento);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes, evento);

            decimal valorSolicitado = await CalcularValorAprovadoAsync(evento);
            if (valorSolicitado == 0)
            {
                evento.ValorCreditoDesejado = 0;
                var propostaRejeitada = CriarProposta(evento, valorSolicitado);
                
                await PublicarPropostaRejeitadaAsync(propostaRejeitada);
                
                return new(false, propostaRejeitada.MotivoRejeicao!, propostaRejeitada);
            }

            var proposta = CriarProposta(evento, valorSolicitado);
            await _propostaRepository.CadastrarAsync(proposta);

            if (proposta.Aprovada)
            {
                await PublishPropostaAprovada(proposta, evento);
                return new(true, "Proposta aprovada!", proposta);
            }
            else
            {
                await PublicarPropostaRejeitadaAsync(proposta);
                return new(false, proposta.MotivoRejeicao ?? "Proposta recusada.", proposta);
            }
        }

        private async Task<decimal> CalcularValorAprovadoAsync(ClienteCadastradoEvent evento)
        {
            decimal limite = evento.RendaMensal * 3;
            decimal solicitacao = evento.ValorCreditoDesejado;
            decimal jaUtilizado = await _propostaRepository.SomarPropostasPorCliente(evento.ClienteId);
            decimal disponivel = limite - jaUtilizado;

            if (disponivel <= 0)
            {
                return 0;
            }

            return solicitacao > disponivel ? disponivel : solicitacao;
        }

        private static Proposta CriarProposta(ClienteCadastradoEvent evento, decimal valorAprovado)
        {
            return new Proposta(evento.ClienteId, valorAprovado, evento.RendaMensal);
        }

        private async Task PublicarPropostaRejeitadaAsync(Proposta proposta)
        {
            var evento = new PropostaCreditoFalhouEvent(proposta.Id, proposta.ClienteId, proposta.MotivoRejeicao!);
            await _publisher.PublishAsync(evento, new("proposta.credito.events", "direct", "proposta.credito.falhou"));
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

        public async Task<PropostaResult?> ObterPropostaPorClienteAsync(Guid clienteId)
        {
            if (clienteId == Guid.Empty)
                return new(false, "ClienteId é obrigatório.", null);

            Proposta? proposta = await _propostaRepository.ObterPorClienteAsync(clienteId);

            if (proposta == null)
                return new(false, "Proposta não encontrada.", null);

            return new(true, "Proposta encontrada!", proposta);
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

        public async Task<PropostaResult> ObterPropostaAsync(Guid propostaId)
        {
            var proposta = await _propostaRepository.ObterPorIdAsync(propostaId);

            if (proposta == null)
            {
                return new PropostaResult(false, "Proposta não encontrada.", null);
            }
            return new PropostaResult(true, "Proposta encontrada!", proposta);
        }
    }
}
