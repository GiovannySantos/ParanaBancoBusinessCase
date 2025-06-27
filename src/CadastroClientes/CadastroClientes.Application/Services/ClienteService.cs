using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Interfaces;
using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;

namespace CadastroClientes.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Guid> CriarAsync(ClienteDto dto)
        {
            //TODO: Validar entrada

            Cliente cliente = new(dto.Nome,dto.Cpf,dto.DataNascimento,dto.Email,dto.Telefone);

            Cliente? a = await _clienteRepository.AdicionarCliente(cliente);

            //TODO: Mensagem em caso de sucesso

            return a.Id;
        }
    }
}
