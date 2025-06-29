namespace PropostaCredito.Application.Messaging
{
    public class PublishProperties(string exchange, string exchangeType, string routingKey, bool mandatory = true, bool durable = true)
    {
        public string Exchange { get; private set; } = exchange;
        public string ExchangeType { get; private set; } = exchangeType;
        public string RoutingKey { get; private set; } = routingKey;
        public bool Mandatory { get; private set; } = mandatory;
        public bool Durable { get; private set; } = durable;
    }
}
