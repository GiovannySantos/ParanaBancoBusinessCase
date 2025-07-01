using PropostaCredito.Domain.Entidades;

namespace PropostaCredito.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task<Proposta> CadastrarAsync(Proposta proposta);
        Task<Proposta?> ObterPorClienteAsync(Guid clienteId);
        Task<Proposta> ObterPorIdAsync(Guid id);
        Task<decimal> SomarPropostasPorCliente(Guid clienteId);
    }
}
