using PropostaCredito.Domain.Entidades;
using PropostaCredito.Domain.Interfaces;
using PropostaCredito.Infra.DbContexts;

namespace PropostaCredito.Infra.Repositories
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly PropostaDbContext _context;
        public PropostaRepository(PropostaDbContext context)
        {
            _context = context;
        }

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

        public async Task<Proposta> ObterPorIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID da proposta não pode ser vazio.", nameof(id));
            }
            var proposta = await _context.Propostas.FindAsync(id);
            if (proposta == null)
            {
                throw new KeyNotFoundException($"Proposta com ID {id} não encontrada.");
            }
            return proposta;
        }
    }
}
