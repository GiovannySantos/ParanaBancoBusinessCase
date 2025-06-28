using CadastroClientes.Application.Messaging;

namespace CadastroClientes.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T evento, PublishProperties publishProperties);
    }
}
