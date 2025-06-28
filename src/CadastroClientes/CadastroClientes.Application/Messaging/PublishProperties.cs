namespace CadastroClientes.Application.Messaging
{
    public class PublishProperties
    {
        public string Exchange { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKey { get; set; }
        public bool Mandatory { get; set; }
        public bool Durable { get; set; }

        public PublishProperties(string exchange, string exchangeType, string routingKey, bool mandatory = true, bool durable = true) 
        {
            Exchange = exchange;
            ExchangeType = exchangeType;
            RoutingKey = routingKey;
            Mandatory = mandatory;
            Durable = durable;
        }
    }
}
