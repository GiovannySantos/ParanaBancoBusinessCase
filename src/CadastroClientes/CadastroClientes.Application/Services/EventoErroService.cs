using CadastroClientes.Application.Interfaces;
using CadastroClientes.Domain.Entidades;
using CadastroClientes.Domain.Interfaces;

namespace CadastroClientes.Application.Services
{
    public class EventoErroService : IEventoErroService
    {
        private readonly IEventoErroRepository _eventoErroRepository;
        public EventoErroService(IEventoErroRepository eventoErroRepository)
        {
            _eventoErroRepository = eventoErroRepository;
        }

        public async Task RegistrarErroAsync(string origem, string routingKey, string payload, string erro)
        {
            var eventoErro = new EventoErro
            {
                Origem = origem,
                RoutingKey = routingKey,
                Payload = payload,
                Erro = erro,
                DataOcorrencia = DateTime.UtcNow
            };
            await _eventoErroRepository.RegistarAsync(eventoErro);
        }
    }
}
