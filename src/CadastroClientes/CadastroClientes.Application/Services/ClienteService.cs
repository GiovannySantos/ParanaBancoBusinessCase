using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Results;
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

        public async Task<ClientesResult> CadastrarAsync(ClienteDto clienteDto)
        {
            bool existe = _clienteRepository.ExistePorCpf(clienteDto.Cpf);
            if (existe)
                return new(false, "Erro ao cadastrar - Já existe um cliente cadastrado com esse CPF");

            Cliente cliente = new(clienteDto.Nome,clienteDto.Cpf,clienteDto.DataNascimento,clienteDto.Email,clienteDto.Telefone);

            Cliente? a = await _clienteRepository.CadastrarAsync(cliente);

            //TODO: Mensagem em caso de sucesso
            return new(sucesso: true, new { a.Nome, a.Email });
        }

        public async Task<ClientesResult> ObterAsync(string cpf)
        {
            bool existe = _clienteRepository.ExistePorCpf(cpf);
            if (!existe)
                return new(false, "Erro ao buscar - Cliente não encontrado");
            var cliente = await _clienteRepository.ObterPorCpf(cpf);

            return new(true, new { cliente.Nome, cliente.Email } );
        }
    }
}
