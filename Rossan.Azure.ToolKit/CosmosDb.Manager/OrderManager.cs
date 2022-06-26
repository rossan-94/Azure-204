using Microsoft.Azure.Cosmos.Scripts;
using Rossan.Azure.CosmosDB;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.Manager
{
    public class OrderManager
    {
        private readonly string _container;
        private readonly string _key;
        public readonly string _database;
        private readonly ICosmosDbRepository<Order> _cosmosDbRepository;

        public OrderManager(ICosmosDbRepository<Order> cosmosDbRepository, string database, string containeName, string partitionKey)
        {
            _cosmosDbRepository = cosmosDbRepository;
            _database = database;
            _container = containeName;
            _key = partitionKey;
        }

        public async Task CreateDatabaseAsync()
        {
            await _cosmosDbRepository.CreateDatabaseAsync(_database).ConfigureAwait(false);
        }

        public async Task CreateConatinerAsync()
        {
            await _cosmosDbRepository.CreateConatinerAsync(_container, _key).ConfigureAwait(false);
        }

        public async Task<Order> CreateAsync(Order entity)
        {
            return await _cosmosDbRepository.CreateAsync<Order>(_container, entity).ConfigureAwait(false);
        }

        public async Task<Order> UpdateAsync(Order entity, string id)
        {
            return await _cosmosDbRepository.UpdateAsync<Order>( _container, entity, id).ConfigureAwait(false);
        }

        public IOrderedQueryable<Order> GetItems()
        {
            return _cosmosDbRepository.GetItems<Order>(_container, _key);
        }

        public Order Get(string id)
        {
            var orders = _cosmosDbRepository.GetItems<Order>(_container, _key).ToList();
            return orders.FirstOrDefault(o => o.Id.Equals(id, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Order> DeleteAsync(string id, string partitionKey)
        {
            // Note detete partition key should be entity partition key value like (Laptop, Mobile, Desktop and Table) for electronics category
            return await _cosmosDbRepository.DeleteAsync<Order>(_container, id, partitionKey).ConfigureAwait(false);
        }

        public async Task<StoredProcedureResponse> CreateStoredProcedureAsync(string storedProcedureName, string body)
        {
            return await _cosmosDbRepository.CreateStoredProcedureAsync(_container, storedProcedureName, body).ConfigureAwait(false);
        }

        public async Task<StoredProcedureResponse> UpdateStoredProcedureAsync(string storedProcedureName, string body)
        {
            return await _cosmosDbRepository.UpdateStoredProcedureAsync(_container, storedProcedureName, body).ConfigureAwait(false);
        }

        public async Task<StoredProcedureResponse> DeleteStoredProcedureAsync(string storedProcedureName)
        {
            return await _cosmosDbRepository.DeleteStoredProcedureAsync(_container, storedProcedureName).ConfigureAwait(false);
        }

        public async Task<StoredProcedureExecuteResponse<Order>> ExecuteStoredProcedureAsync(string storedProcedureName, string partionKey, dynamic[] orders)
        {
            return await _cosmosDbRepository.ExecuteStoredProcedureAsync<Order>(_container, storedProcedureName, partionKey, orders).ConfigureAwait(false);
        }

    }
}
