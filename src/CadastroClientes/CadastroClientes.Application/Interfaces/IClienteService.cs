using CadastroClientes.Application.DTOs;

namespace CadastroClientes.Application.Interfaces
{
    public interface IClienteService
    {
        Task<Guid> CriarClienteAsync(ClienteDto dto);
    }
}
