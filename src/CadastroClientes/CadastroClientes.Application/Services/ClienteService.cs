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
        private readonly IEventPublisher _publisher;

        public ClienteService(IClienteRepository clienteRepository, IEventPublisher eventPublisher)
        {
            _clienteRepository = clienteRepository;
            _publisher = eventPublisher;
        }

        public async Task<ClientesResult> CadastrarAsync(ClienteDto clienteDto)
        {
            bool existe = _clienteRepository.ExistePorCpf(clienteDto.Cpf);
            if (existe)
                return new(false, "Erro ao cadastrar - Já existe um cliente cadastrado com esse CPF");

            Cliente? cliente = await _clienteRepository.CadastrarAsync(new(clienteDto.Nome, clienteDto.Cpf, clienteDto.DataNascimento, clienteDto.Email, clienteDto.Telefone));

            if (cliente == null)
                return new(false, "Erro ao cadastrar - Cliente não pôde ser cadastrado");

            await _publisher.PublishClienteCadastrado(new(
                 clienteId: cliente.Id,
                 nome: cliente.Nome,
                 cpf: cliente.Cpf,
                 dataNascimento: cliente.DataNascimento,
                 email: cliente.Email,
                 telefone: cliente.Telefone
            ));

            return new(sucesso: true, new { cliente.Nome, cliente.Email });
        }

        public async Task<ClientesResult> ObterAsync(string cpf)
        {
            bool existe = _clienteRepository.ExistePorCpf(cpf);
            if (!existe)
                return new(false, "Erro ao buscar - Cliente não encontrado");
            var cliente = await _clienteRepository.ObterPorCpf(cpf);

            return new(true, new { cliente.Nome, cliente.Email });
        }
    }
}
