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

        public async Task<Guid> CriarAsync(ClienteDto clienteDto)
        {
            Cliente cliente = new(
                clienteDto.Nome,
                clienteDto.Cpf,
                clienteDto.DataNascimento,
                clienteDto.Email,
                clienteDto.Telefone
            );

            var a = await _clienteRepository.AdicionarCliente(cliente);

            return a.Id;
        }
    }
}
