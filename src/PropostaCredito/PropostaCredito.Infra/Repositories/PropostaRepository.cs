using Microsoft.EntityFrameworkCore;
using PropostaCredito.Domain.Entidades;
using PropostaCredito.Domain.Interfaces;
using PropostaCredito.Infra.DbContexts;

namespace PropostaCredito.Infra.Repositories
{
    public class PropostaRepository(PropostaDbContext context) : IPropostaRepository
    {
        private readonly PropostaDbContext _context = context;

        public async Task<Proposta> CadastrarAsync(Proposta proposta)
        {
            if (proposta == null)
            {
                throw new ArgumentNullException(nameof(proposta), "Proposta não pode ser nula.");
            }

            await _context.Propostas.AddAsync(proposta);
            await _context.SaveChangesAsync();
            return proposta;
        }

        public Task<Proposta?> ObterPorClienteAsync(Guid clienteId)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> SomarPropostasPorCliente(Guid clienteId)
        {
            return await _context.Propostas
                .Where(p => p.ClienteId == clienteId && p.Aprovada)
                .SumAsync(p => p.ValorSolicitado);
        }

        public async Task<Proposta> ObterPorIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID da proposta não pode ser vazio.", nameof(id));
            }

            var proposta = await _context.Propostas.FindAsync(id);

            return proposta ?? throw new KeyNotFoundException($"Proposta com ID {id} não encontrada.");
        }

        
    }
}
