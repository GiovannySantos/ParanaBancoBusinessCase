namespace CadastroClientes.Domain.Entidades
{
    public class EventoErro
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Origem { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public string Erro { get; set; } = string.Empty;
        public DateTime DataOcorrencia { get; set; }
    }
}
