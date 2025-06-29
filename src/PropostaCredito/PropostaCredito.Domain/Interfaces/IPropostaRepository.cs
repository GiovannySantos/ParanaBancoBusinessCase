using PropostaCredito.Domain.Entidades;

namespace PropostaCredito.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task<Proposta> CadastrarAsync(Proposta proposta);
        Task<Proposta> ObterPorIdAsync(Guid id);
    }
}
