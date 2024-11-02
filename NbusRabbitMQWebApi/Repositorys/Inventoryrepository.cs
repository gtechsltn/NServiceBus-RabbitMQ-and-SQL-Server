using Dapper;
using NbusRabbitMQWebApi.Data;
using System.Data;

namespace NbusRabbitMQWebApi.Repositorys
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DapperContext _dbContext;

        public InventoryRepository(DapperContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckAvailability(string productName)
        {
            var sql = "SELECT Stock FROM Inventory WHERE ProductName = @ProductName";
            using var _dbConnection = _dbContext.CreateConnection();
            int stock = await _dbConnection.QuerySingleOrDefaultAsync<int>(sql, new { ProductName = productName });
            return stock > 0;
        }

        public async Task ReserveStock(string productName)
        {
            var sql = "UPDATE Inventory SET Stock = Stock - 1 WHERE ProductName = @ProductName AND Stock > 0";

            using var _dbConnection = _dbContext.CreateConnection();
            int rowsAffected = await _dbConnection.ExecuteAsync(sql, new { ProductName = productName });

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Unable to reserve stock, product is out of stock.");
            }
        }
    }
}
