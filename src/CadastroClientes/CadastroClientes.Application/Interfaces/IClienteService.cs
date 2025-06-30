using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Results;
using CadastroClientes.Domain.Events.Consumers;

namespace CadastroClientes.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClientesResult> CadastrarAsync(ClienteDto clienteDto);
        Task<ClientesResult> ObterAsync(string cpf);
        Task VincularCartaoClienteAsync(CartaoCreditoCriadoEvent evento);
    }
}
