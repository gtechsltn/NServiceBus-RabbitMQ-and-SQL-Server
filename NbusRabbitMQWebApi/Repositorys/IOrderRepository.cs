using NbusRabbitMQWebApi.Models;

namespace NbusRabbitMQWebApi.Repositorys
{
    public interface IOrderRepository
    {
        Task SaveOrderAsync(Order order);
        Task UpdateOrderStatusAsync(Order order);
    }
}