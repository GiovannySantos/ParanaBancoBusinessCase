using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Results;

namespace CadastroClientes.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClientesResult> CadastrarAsync(ClienteDto clienteDto);
        Task<ClientesResult> ObterAsync(string cpf);
    }
}
