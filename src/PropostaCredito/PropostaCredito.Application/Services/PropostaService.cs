using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Application.Results;
using PropostaCredito.Domain.Interfaces;

namespace PropostaCredito.Application.Services
{
    public class PropostaService : IPropostaService
    {

        private readonly IPropostaRepository _propostaRepository;
        public PropostaService(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;
        }

        public async Task<PropostaResult> CadastrarAsync(PropostaDto propostaDto)
        {
            var validacoes = ValidarPropostaDto(propostaDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            var proposta = await _propostaRepository.CadastrarAsync(new(propostaDto.ClienteId, propostaDto.ValorSolicitado, propostaDto.RendaMensal));

            //Notificar os serviços que a proposta foi criada (aprovada ou não)

            return new(true, proposta);
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