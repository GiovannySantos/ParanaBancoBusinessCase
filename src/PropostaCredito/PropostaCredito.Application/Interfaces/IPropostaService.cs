using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Results;

namespace PropostaCredito.Application.Interfaces
{
    public interface IPropostaService
    {
        Task<PropostaResult> CadastrarAsync(PropostaDto propostaDto);

        Task<PropostaResult> ObterPropostaAsync(Guid idProposta);
    }
}
