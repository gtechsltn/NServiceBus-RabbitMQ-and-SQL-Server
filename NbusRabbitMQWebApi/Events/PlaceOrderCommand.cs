namespace NbusRabbitMQWebApi.Events
{  

    public class OrderPlacedEvent : IEvent
    {
        public int OrderId { get; set; }

        public string? ProductName { get; set; }
    }
}
