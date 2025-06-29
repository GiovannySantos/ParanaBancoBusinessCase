using CartaoCredito.Infra.DbContexts;
using CartaoDeCredito.Domain.Interfaces;

namespace CartaoCredito.Infra.Repositories
{
    public class CartaoCreditoRepository : ICartaoCreditoRepository
    {
        private readonly CartaoCreditoDbContext _context;

        public CartaoCreditoRepository(CartaoCreditoDbContext context)
        {
            _context = context; 
        }

        public async Task<CartaoDeCredito.Domain.Entidades.CartaoCredito> CadastrarAsync(CartaoDeCredito.Domain.Entidades.CartaoCredito cartaoCredito)
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
