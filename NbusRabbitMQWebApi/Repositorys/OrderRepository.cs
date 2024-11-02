using Dapper;
using NbusRabbitMQWebApi.Data;
using NbusRabbitMQWebApi.Models;
using System.Data;

namespace NbusRabbitMQWebApi.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _dbContext;

        public OrderRepository(DapperContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveOrderAsync(Order order)
        {
            var sql = "INSERT INTO Orders (OrderId, ProductName) VALUES (@OrderId, @ProductName)";
            using var _dbConnection = _dbContext.CreateConnection();
            await _dbConnection.ExecuteAsync(sql, order);
        }

        public async Task UpdateOrderStatusAsync(Order order)
        {
            var sql = "UPDATE Orders SET ProductName = @ProductName WHERE OrderId = @OrderId";
            using var _dbConnection = _dbContext.CreateConnection();
            await _dbConnection.ExecuteAsync(sql, new { order.ProductName, order.OrderId });
        }
    }
}
