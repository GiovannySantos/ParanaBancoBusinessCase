using CadastroClientes.Domain.Entidades;

namespace CadastroClientes.Domain.Interfaces
{
    public interface IClienteCartaoRepository
    {
        Task<ClienteCartao> AdicionarAsync(ClienteCartao clienteCartao);
        Task<ClienteCartao?> ObterPorCartaoId(Guid cartaoId);
    }
}
