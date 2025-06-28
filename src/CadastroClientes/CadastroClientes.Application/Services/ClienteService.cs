using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Results;
using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Domain.Events;

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

            // Publica o evento de cliente cadastrado
            var cadastroClienteEvent = new ClienteCadastradoEvent(cliente.Id,cliente.Nome,cliente.Cpf,cliente.DataNascimento,cliente.Email,cliente.Telefone);
            await _publisher.PublishAsync(cadastroClienteEvent, new("cliente.cadastrado.exchange", "direct","cliente.cadastrado"));

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
