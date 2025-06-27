using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.Data;

namespace CadastroClientes.Infra.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CadastroClientesDbContext _context;

        public ClienteRepository(CadastroClientesDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> AdicionarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }
    }
}
