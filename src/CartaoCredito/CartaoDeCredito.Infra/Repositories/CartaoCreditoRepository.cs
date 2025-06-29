using CartaoCredito.Domain.Interfaces;
using CartaoCredito.Infra.DbContexts;

namespace CartaoCredito.Infra.Repositories
{
    public class CartaoCreditoRepository(CartaoCreditoDbContext context) : ICartaoCreditoRepository
    {
        private readonly CartaoCreditoDbContext _context = context;

        public async Task<Domain.Entidades.CartaoCredito> CadastrarAsync(Domain.Entidades.CartaoCredito cartaoCredito)
        {
            if (cartaoCredito == null)
            {
                throw new ArgumentNullException(nameof(cartaoCredito), "Cartão de crédito não pode ser nulo.");
            }
            await _context.CartoesCredito.AddAsync(cartaoCredito);
            await _context.SaveChangesAsync();
            return cartaoCredito;
        }
    }
}
