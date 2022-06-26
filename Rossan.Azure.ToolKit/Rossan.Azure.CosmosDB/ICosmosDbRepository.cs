using Microsoft.Azure.Cosmos.Scripts;
using System.Linq;
using System.Threading.Tasks;

namespace Rossan.Azure.CosmosDB
{
    public interface ICosmosDbRepository<TEntity> where TEntity : class
    {
        IOrderedQueryable<T> GetItems<T>(string containerName, string partiotionKey);

        Task<T> CreateAsync<T>(string containerName, T entity);

        Task<T> UpdateAsync<T>(string containerName, T entity, string id);

        Task<T> DeleteAsync<T>(string containerName, string id, string partiotionKey);

        Task CreateDatabaseAsync(string database);

        Task CreateConatinerAsync(string conatinerName, string partitionKey);

        Task<StoredProcedureResponse> CreateStoredProcedureAsync(string containerName, string storedProcedureName, string body);

        Task<StoredProcedureResponse> UpdateStoredProcedureAsync(string containerName, string storedProcedureName, string body);

        Task<StoredProcedureExecuteResponse<T>> ExecuteStoredProcedureAsync<T>(string containerName, string storedProcedureName, string partitionKey, dynamic[] body);

        Task<StoredProcedureResponse> DeleteStoredProcedureAsync(string containerName, string storedProcedureName);
    }
}
