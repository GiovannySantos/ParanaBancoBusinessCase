
namespace CadastroClientes.Application.Interfaces
{
    public interface IEventoErroService
    {
        Task RegistrarErroAsync(string origem, string routingKey, string payload, string erro);
    }
}
