using CadastroClientes.Application.DTOs;
using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Infra.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CadastroClientesDbContext _context;

        public ClienteRepository(CadastroClientesDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> CadastrarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public bool ExistePorCpf(string cpf) => _context.Clientes.Any(x => x.Cpf == cpf);

        public async Task<Cliente> ObterPorCpf(string cpf) => await _context.Clientes.FirstOrDefaultAsync(x => x.Cpf == cpf);
    }
}
