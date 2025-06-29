using CartaoDeCredito.Domain.Entidades;
using CartaoDeCredito.Domain.Interfaces;
using CartaoDeCredito.Infra.DbContexts;

namespace CartaoDeCredito.Infra.Repositories
{
    public class CartaoCreditoRepository : ICartaoCreditoRepository
    {
        private readonly CartaoCreditoDbContext _context;

        public CartaoCreditoRepository(CartaoCreditoDbContext context)
        {
            _context = context; 
        }

        public async Task<CartaoCredito> CadastrarAsync(CartaoCredito cartaoCredito)
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
