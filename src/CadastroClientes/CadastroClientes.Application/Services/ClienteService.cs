using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Results;
using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Events.Consumers;
using CadastroClientes.Domain.Events.Publishers;
using CadastroClientes.Domain.Interfaces;

namespace CadastroClientes.Application.Services
{
    public class ClienteService(IClienteRepository clienteRepository, IClienteCartaoRepository clienteCartaoRepository, IEventPublisher eventPublisher) : IClienteService
    {
        private readonly IClienteRepository _clienteRepository = clienteRepository;
        private readonly IClienteCartaoRepository _clienteCartaoRepository = clienteCartaoRepository;
        private readonly IEventPublisher _publisher = eventPublisher;

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

            var cliente = await _clienteRepository.ObterPorCpf(cpf);
            if (cliente is null)
                return new(false, "Cliente não encontrado.");

            return new(true, new { cliente });
        }

        public async Task VincularCartaoClienteAsync(CartaoCreditoCriadoEvent evento)
        {
            Cliente? cliente = await _clienteRepository.ObterPorId(evento.ClienteId);
            if (cliente is null)
                throw new Exception("Cliente não encontrado para vincular o cartão de crédito.");

            // Verifica se o cartão já está vinculado ao cliente
            var cartaoExistente = await _clienteCartaoRepository.ObterPorCartaoId(evento.CartaoId);
            if (cartaoExistente != null)
                return;


            await _clienteCartaoRepository.AdicionarAsync(new ClienteCartao(
                evento.ClienteId,
                evento.PropostaId,
                evento.CartaoId,
                evento.NumeroCartao,
                evento.NomeImpresso,
                evento.Validade,
                evento.Limite
            ));
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
