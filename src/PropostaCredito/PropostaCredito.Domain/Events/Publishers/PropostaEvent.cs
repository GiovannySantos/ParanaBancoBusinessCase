namespace PropostaCredito.Domain.Events.Publishers
{
    public class PropostaEvent(Guid id, Guid clienteId,string nomeCliente, decimal valorAprovado, bool aprovado, string motivoRejeicao)
    {
        public Guid Id { get; } = id;
        public Guid ClienteId { get; } = clienteId;
        public string NomeCliente { get; } = nomeCliente;
        public decimal ValorAprovado { get; } = valorAprovado;
        public bool Aprovado { get; } = aprovado;
        public string? MotivoRejeicao { get; } = motivoRejeicao;
    }
}
