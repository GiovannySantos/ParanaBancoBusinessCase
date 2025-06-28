namespace CadastroClientes.Application.Messaging
{
    public class PublishProperties
    {
        public string Exchange { get; set; } = "cadastro.clientes.exchange";
        public string ExchangeType { get; set; } = "direct";
        public string RoutingKey { get; set; } = "cliente.cadastrado";
        public bool Mandatory { get; set; } = true;
        public bool Durable { get; set; } = true;

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
