
namespace NbusRabbitMQWebApi.Services
{
    public interface IOrderService
    {
        Task PlaceOrderAsync(int orderId, string productName);
    }
}