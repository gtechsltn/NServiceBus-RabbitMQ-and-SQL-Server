
namespace NbusRabbitMQWebApi.Repositorys
{
    public interface IInventoryRepository
    {
        Task<bool> CheckAvailability(string productName);
        Task ReserveStock(string productName);
    }
}