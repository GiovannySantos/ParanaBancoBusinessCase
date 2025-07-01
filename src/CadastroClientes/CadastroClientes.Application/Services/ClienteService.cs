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
                return new(false, validacoes, clienteDto);

            //validar se o cliente já existe (por cpf)
            //Caso exista: reutiliza todos os campos e solicita uma nova proposta com o valor solicitado para o mesmo cliente
            Cliente? cliente = await _clienteRepository.ObterPorCpf(clienteDto.Cpf);
            if (cliente == null)
            {
                cliente = await _clienteRepository.CadastrarAsync(new(
                    clienteDto.Nome,
                    clienteDto.Cpf,
                    clienteDto.DataNascimento,
                    clienteDto.Email,
                    clienteDto.Telefone,
                    clienteDto.RendaMensal,
                    clienteDto.ValorCreditoDesejado
                ));
                await PublishClienteCadastrado(cliente);
                return new(sucesso: true, "Cliente cadastrado com sucesso!", clienteDto);
            }

            cliente.ValorCreditoDesejado = clienteDto.ValorCreditoDesejado;

            //Se a renda mensal mudou atribui o novo valor
            if (cliente.RendaMensal != clienteDto.RendaMensal)
            {
                cliente.RendaMensal = clienteDto.RendaMensal;
                await _clienteRepository.AtualizarAsync(cliente);
            }

            await PublishClienteCadastrado(cliente);


            return new(sucesso: true, $"Cliente já possui cadastro, foi enviado uma análise da proposta no valor de {clienteDto.ValorCreditoDesejado}", clienteDto);
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
                return new(false, "Cliente não encontrado para o cpf informado", new());

            return new(true, "Cliente obtido com sucesso!", new
            {
                cliente.Nome,
                DataNascimento = cliente.DataNascimento.ToString("dd/MM/yyyy"),
                cliente.Cpf,
                cliente.Email,
                cliente.Telefone
            });
        }

        public async Task VincularCartaoClienteAsync(CartaoCreditoCriadoEvent evento)
        {
            _ = await _clienteRepository.ObterPorId(evento.ClienteId)
                ?? throw new Exception("Cliente não encontrado para vincular o cartão de crédito.");

            // Verifica se o cartão já está vinculado ao cliente
            ClienteCartao? cartaoExistente = await _clienteCartaoRepository.ObterPorCartaoId(evento.CartaoId);
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

            if (!PossuiIdadeMinima(clienteDto.DataNascimento))
                erros.Add("Cliente deve ter pelo menos 18 anos.");

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

        private static bool PossuiIdadeMinima(DateTime dataNascimento)
        {
            int idade = DateTime.Today.Year - dataNascimento.Year;
            if (dataNascimento > DateTime.Today.AddYears(-idade))
                idade--;
            return idade >= 18;
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
