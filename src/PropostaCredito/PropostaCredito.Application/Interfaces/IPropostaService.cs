using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Results;
using PropostaCredito.Domain.Events.Consumers;

namespace PropostaCredito.Application.Interfaces
{
    public interface IPropostaService
    {
        Task<PropostaResult> InserirAsync(PropostaDto propostaDto);
        Task<PropostaResult> CadastrarAsync(ClienteCadastradoEvent propostaDto);
        Task<PropostaResult> ObterPropostaAsync(Guid idProposta);
    }
}
