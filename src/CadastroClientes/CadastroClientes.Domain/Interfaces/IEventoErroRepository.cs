using CadastroClientes.Domain.Entidades;

namespace CadastroClientes.Domain.Interfaces
{
    public interface IEventoErroRepository
    {
        Task RegistarAsync(EventoErro eventoErro);
    }
}
