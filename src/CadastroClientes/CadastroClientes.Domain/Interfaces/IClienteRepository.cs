using CadastroClientes.Domain.Entidades;

namespace CadastroClientes.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> AdicionarCliente(Cliente cliente);
    }
}
