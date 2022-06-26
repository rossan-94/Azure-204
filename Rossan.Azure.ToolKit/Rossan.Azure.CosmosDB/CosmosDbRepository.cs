using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rossan.Azure.CosmosDB
{
    public class CosmosDbRepository<TEntity> : ICosmosDbRepository<TEntity> where TEntity : class
    {
        private readonly CosmosClient _cosmosClient;
        private Database _databaseClient;

        public CosmosDbRepository(string cosmosEndpointUri, string cosmosKey, string database)
        {
            _cosmosClient = new CosmosClient(cosmosEndpointUri, cosmosKey);
            _databaseClient = _cosmosClient.GetDatabase(database);
        }

        public virtual async Task CreateDatabaseAsync(string database)
        {
            await _cosmosClient.CreateDatabaseIfNotExistsAsync(database).ConfigureAwait(false);
            _databaseClient = _cosmosClient.GetDatabase(database);
        }

        public virtual async Task CreateConatinerAsync(string containerName, string partitionKey)
        {
            await _databaseClient.CreateContainerIfNotExistsAsync(containerName, partitionKey).ConfigureAwait(false);
        }

        public virtual async Task<T> CreateAsync<T>(string containerName, T entity)
        {
            var containerClient = _databaseClient.GetContainer(containerName);
            // await containerClient.CreateItemAsync(entity, new PartitionKey("partitionKey")).ConfigureAwait(false); 
            return await containerClient.CreateItemAsync<T>(entity).ConfigureAwait(false);
        }

        public virtual async Task<T> DeleteAsync<T>(string containerName, string id, string partiotionKey)
        {
            var containerClient = _databaseClient.GetContainer(containerName);
            return await containerClient.DeleteItemAsync<T>(id, new PartitionKey(partiotionKey)).ConfigureAwait(false);
        }

        public virtual IOrderedQueryable<T> GetItems<T>(string containerName, string partiotionKey)
        {
            var containerClient = _databaseClient.GetContainer(containerName);
            return containerClient.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: true, continuationToken: null, requestOptions: null, linqSerializerOptions: null);
        }

        public virtual async Task<T> UpdateAsync<T>(string containerName, T entity, string id)
        {
            var containerClient = _databaseClient.GetContainer(containerName);
            return await containerClient.ReplaceItemAsync<T>(entity, id).ConfigureAwait(false);
        }

        public virtual async Task<StoredProcedureResponse> CreateStoredProcedureAsync(string containerName, string storedProcedureName, string body)
        {
            var containerClinet = _databaseClient.GetContainer(containerName);
            var storeProcedureProperties = new StoredProcedureProperties(storedProcedureName, body);
            return await containerClinet.Scripts.CreateStoredProcedureAsync(storeProcedureProperties).ConfigureAwait(false);
        }

        public virtual async Task<StoredProcedureResponse> UpdateStoredProcedureAsync(string containerName, string storedProcedureName, string body)
        {
            // Note it is not working need some research on it
            var containerClinet = _databaseClient.GetContainer(containerName);
            var storeProcedureProperties = new StoredProcedureProperties(storedProcedureName, body);
            return await containerClinet.Scripts.ReplaceStoredProcedureAsync(storeProcedureProperties).ConfigureAwait(false);
        }

        public virtual async Task<StoredProcedureResponse> DeleteStoredProcedureAsync(string containerName, string storedProcedureName)
        {
            var containerClinet = _databaseClient.GetContainer(containerName);
            return await containerClinet.Scripts.DeleteStoredProcedureAsync(storedProcedureName).ConfigureAwait(false);
        }

        public async Task<StoredProcedureExecuteResponse<T>> ExecuteStoredProcedureAsync<T>(string containerName, string storedProcedureName, string partitionKey, dynamic[] body)
        {
            var containerClinet = _databaseClient.GetContainer(containerName);
            return await containerClinet.Scripts.ExecuteStoredProcedureAsync<T>(storedProcedureName, new PartitionKey(partitionKey), body).ConfigureAwait(false);
        }
    }
}
