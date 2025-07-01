using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Infra.Repositories
{
    public class ClienteRepository(CadastroClientesDbContext context) : IClienteRepository
    {
        private readonly CadastroClientesDbContext _context = context;

        public Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            return _context.SaveChangesAsync();
        }

        public async Task<Cliente> CadastrarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> ObterPorCpf(string cpf) => await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);

        public async Task<Cliente?> ObterPorId(Guid clienteId) => await _context.Clientes.FindAsync(clienteId);
    }
}
