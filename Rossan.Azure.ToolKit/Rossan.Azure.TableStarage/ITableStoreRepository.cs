using Azure;
using Azure.Data.Tables;

namespace Rossan.Azure.TableStarage
{
    public interface ITableStoreRepository
    {
        Task CreateAsync<T>(T data, string tableName = "")
            where T : ITableEntity;

        Task CreateEntityGroupAsync<T>(IEnumerable<T> data, string tableName = "")
            where T : ITableEntity;

        Task<Pageable<T>> GetAsync<T>(
            string partitionKey,
            int pageSize = 0,
            string? continuationToken = null,
            string tableName = "")
            where T : class, ITableEntity, new();

        Task<T> GetItemAsync<T>(
           string partitionKey,
           string rowKey,
           string tableName = "")
           where T : class, ITableEntity, new();

        Task UpdateAsync<T>(T data, string tableName = "")
            where T : class, ITableEntity, new();

        Task UpdateEntityGroupAsync<T>(IEnumerable<T> data, string tableName = "")
           where T : class, ITableEntity, new();

        Task DeleteAsync<T>(string partitionKey, string rowKey, string tableName = "")
            where T : ITableEntity;

        Task DeleteEntityGroupAsync<T>(IEnumerable<T> data, string tableName = "")
            where T : ITableEntity;
    }
}
