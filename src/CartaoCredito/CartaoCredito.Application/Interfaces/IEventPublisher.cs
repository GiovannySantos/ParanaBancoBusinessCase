using CartaoCredito.Application.Messaging;

namespace CartaoCredito.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T evento, PublishProperties publishProperties);
    }
}
