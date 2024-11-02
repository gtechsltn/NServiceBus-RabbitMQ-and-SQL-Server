using NbusRabbitMQWebApi.Commands;
using NbusRabbitMQWebApi.Events;
using NbusRabbitMQWebApi.Models;
using NbusRabbitMQWebApi.Repositorys;

namespace NbusRabbitMQWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryRepository _inventoryReposiotory;
        //private readonly IMessageSession _messageSession; // Optional for publishing events
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IInventoryRepository inventoryReposiotory,
           // IMessageSession messageSession,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _inventoryReposiotory = inventoryReposiotory;
           // _messageSession = messageSession;
            _logger = logger;
        }

        public async Task PlaceOrderAsync(int orderId, string productName)
        {
            try
            {
                // Step 1: Validate order
                if (!await _inventoryReposiotory.CheckAvailability(productName))
                {
                    throw new InvalidOperationException("Product is out of stock.");
                }

                // Step 2: Create and save the order
                var order = new Order
                {
                    OrderId = orderId,
                    ProductName = productName
                };
                await _orderRepository.SaveOrderAsync(order);

                // Step 3: Update inventory
                await _inventoryReposiotory.ReserveStock(productName);

                // Step 4: Update order status to 'Processed'
                //order.Status = "Processed";
                await _orderRepository.UpdateOrderStatusAsync(order);

                _logger.LogInformation($"Order {orderId} processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process order {orderId}");
                throw; // Or handle exceptions as per the application requirement
            }
        }


    }
}
