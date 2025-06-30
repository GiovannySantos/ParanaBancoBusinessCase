using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;
using CadastroClientes.Infra.DbContexts;

namespace CadastroClientes.Infra.Repositories
{
    public class EventoErroRepository(CadastroClientesDbContext context) : IEventoErroRepository
    {
        private readonly CadastroClientesDbContext _context = context;

        public async Task RegistarAsync(EventoErro eventoErro)
        {
            await _context.EventosErros.AddAsync(eventoErro);
            await _context.SaveChangesAsync();
        }
    }
}
