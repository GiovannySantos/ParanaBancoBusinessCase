using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Interfaces;
using PropostaCredito.Application.Results;

namespace PropostaCredito.Application.Services
{
    public class PropostaService : IPropostaService
    {
        public async Task<PropostaResult> CadastrarAsync(PropostaDto propostaDto)
        {
            var validacoes = ValidarPropostaDto(propostaDto);

            return new();
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