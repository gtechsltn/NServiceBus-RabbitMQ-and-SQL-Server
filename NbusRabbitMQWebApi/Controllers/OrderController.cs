using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NbusRabbitMQWebApi.Commands;
using NbusRabbitMQWebApi.Events;
using NbusRabbitMQWebApi.Handlers;
using NbusRabbitMQWebApi.Services;

namespace NbusRabbitMQWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMessageSession _messageSession;
        private readonly IOrderService _orderService;

        public OrderController(IMessageSession messageSession, IOrderService orderService)
        {
            _messageSession = messageSession;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int orderId, string productName)
        {
            var command = new PlaceOrderCommand
            {
                OrderId = orderId,
                ProductName = productName
            };

            await _messageSession.Send(command);

            return Ok("Order command sent successfully.");
        }
    }
}
