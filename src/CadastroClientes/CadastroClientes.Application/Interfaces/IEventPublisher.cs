namespace CadastroClientes.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishClienteCadastrado(ClienteCadastradoEvent clienteCadastradoEvent);
    }
}
