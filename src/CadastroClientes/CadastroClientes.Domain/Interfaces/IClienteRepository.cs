using CadastroClientes.Domain.Entidades;

namespace CadastroClientes.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> CadastrarAsync(Cliente cliente);
        Task<Cliente?> ObterPorCpf(string cpf);
        Task<Cliente?> ObterPorId(Guid clienteId);
    }
}
