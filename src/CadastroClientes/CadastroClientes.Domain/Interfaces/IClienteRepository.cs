using CadastroClientes.Domain.Entidades;

namespace CadastroClientes.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> CadastrarAsync(Cliente cliente);
        bool ExistePorCpf(string cpf);
        Task<Cliente> ObterPorCpf(string cpf);
    }
}
