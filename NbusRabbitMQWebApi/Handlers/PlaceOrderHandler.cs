using NbusRabbitMQWebApi.Commands;
using NbusRabbitMQWebApi.Events;
using NbusRabbitMQWebApi.Services;

namespace NbusRabbitMQWebApi.Handlers
{
    public class PlaceOrderCommandHandler : IHandleMessages<PlaceOrderCommand>
    {
        public async Task Handle(PlaceOrderCommand message, IMessageHandlerContext context)
        {
            try
            {
                // Retrieve the service from the ServiceLocator
                var _orderService = ServiceLocator.ServiceProvider.GetRequiredService<IOrderService>(); 

                Console.WriteLine($"Handling PlaceOrderCommand: OrderId={message.OrderId}, ProductName={message.ProductName}");
                if (message != null)
                {

                    await _orderService.PlaceOrderAsync(message.OrderId, message.ProductName);
                }
            }
            catch (Exception ex)
            {
            }
            await Task.CompletedTask;
        }

    }
}
