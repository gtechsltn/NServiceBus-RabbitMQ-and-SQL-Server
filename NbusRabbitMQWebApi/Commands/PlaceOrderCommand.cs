namespace NbusRabbitMQWebApi.Commands
{
    public class PlaceOrderCommand : ICommand
    {
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
    }
}
