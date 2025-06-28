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
            var validacoes = ValidarClienteDto(clienteDto);
            if (validacoes != null && validacoes.Count > 0)
                return new(false, validacoes);

            Cliente? cliente = await _clienteRepository.CadastrarAsync(new(
                clienteDto.Nome,
                clienteDto.Cpf,
                clienteDto.DataNascimento,
                clienteDto.Email,
                clienteDto.Telefone,
                clienteDto.RendaMensal,
                clienteDto.ValorCreditoDesejado
            ));

            // Publica o evento de cliente cadastrado
            await PublishClienteCadastrado(cliente);

            return new(sucesso: true, new { cliente.Nome, cliente.Email });
        }

        private async Task PublishClienteCadastrado(Cliente cliente)
        {
            ClienteCadastradoEvent cadastroClienteEvent = new(
               cliente.Id,
               cliente.Nome,
               cliente.Cpf,
               cliente.DataNascimento,
               cliente.Email,
               cliente.Telefone,
               cliente.RendaMensal,
               cliente.ValorCreditoDesejado
            );
            await _publisher.PublishAsync(cadastroClienteEvent, new("cadastro.clientes.events", "direct", "cliente.cadastrado"));
        }

        public async Task<ClientesResult> ObterAsync(string cpf)
        {
            bool existe = _clienteRepository.ExistePorCpf(cpf);
            if (!existe)
                return new(false, "Erro ao buscar - Cliente não encontrado");
            var cliente = await _clienteRepository.ObterPorCpf(cpf);

            return new(true, new { cliente.Nome, cliente.Email });
        }

        #region Validações do Cliente

        private static List<string>? ValidarClienteDto(ClienteDto clienteDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(clienteDto.Nome))
                erros.Add("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(clienteDto.Cpf) || !ValidarCpf(clienteDto.Cpf))
                erros.Add("CPF é inválido ou está vazio.");

            if (clienteDto.DataNascimento == default || clienteDto.DataNascimento > DateTime.Today)
                erros.Add("Data de nascimento inválida.");

            if (string.IsNullOrWhiteSpace(clienteDto.Email) || !ValidarEmail(clienteDto.Email))
                erros.Add("Email é inválido ou está vazio.");

            if (string.IsNullOrWhiteSpace(clienteDto.Telefone) || !ValidarTelefone(clienteDto.Telefone))
                erros.Add("Telefone é inválido ou está vazio.");

            if (clienteDto.RendaMensal <= 0)
                erros.Add("Renda mensal deve ser maior que zero.");

            if (clienteDto.ValorCreditoDesejado <= 0)
                erros.Add("Valor do crédito desejado deve ser maior que zero.");

            return erros.Count > 0 ? erros : null;
        }

        private static bool ValidarCpf(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = new string([.. cpf.Where(char.IsDigit)]);

            if (cpf.Length != 11) return false;

            // Pode adicionar validação de dígito verificador aqui (mais avançada)
            return true;
        }

        private static bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool ValidarTelefone(string telefone)
        {
            // Telefone só números, mínimo 8 e máximo 15 dígitos, por exemplo
            var numeros = new string([.. telefone.Where(char.IsDigit)]);
            return numeros.Length >= 8 && numeros.Length <= 15;
        }

        #endregion
    }
}
