using PropostaCredito.Application.Messaging;

namespace PropostaCredito.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T evento, PublishProperties publishProperties);
    }
}
