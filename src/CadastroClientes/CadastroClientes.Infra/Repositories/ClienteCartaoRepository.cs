using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Infra.Repositories
{
    public class ClienteCartaoRepository(CadastroClientesDbContext context) : IClienteCartaoRepository
    {
        private readonly CadastroClientesDbContext _context = context;

        public async Task<ClienteCartao> AdicionarAsync(ClienteCartao clienteCartao)
        {
            await _context.ClienteCartoes.AddAsync(clienteCartao);
            await _context.SaveChangesAsync();
            return clienteCartao;
        }

        public async Task<ClienteCartao?> ObterPorCartaoId(Guid cartaoId)
        {
            return await _context.ClienteCartoes.Where(x=> x.CartaoId == cartaoId).FirstOrDefaultAsync();
        }
    }
}
