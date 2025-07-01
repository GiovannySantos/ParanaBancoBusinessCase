using CadastroClientes.Domain.Entidades;

namespace CadastroClientes.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task AtualizarAsync(Cliente cliente);
        Task<Cliente> CadastrarAsync(Cliente cliente);
        Task<Cliente?> ObterPorCpf(string cpf);
        Task<Cliente?> ObterPorId(Guid clienteId);
    }
}
