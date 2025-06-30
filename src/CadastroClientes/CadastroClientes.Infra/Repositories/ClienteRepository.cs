using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;

namespace CadastroClientes.Infra.Repositories
{
    public class ClienteRepository(CadastroClientesDbContext context) : IClienteRepository
    {
        private readonly CadastroClientesDbContext _context = context;

        public async Task<Cliente> CadastrarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> ObterPorCpf(string cpf) => await _context.Clientes.FindAsync(cpf);

        public async Task<Cliente?> ObterPorId(Guid clienteId) => await _context.Clientes.FindAsync(clienteId);
    }
}
