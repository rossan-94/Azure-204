using Azure;
using Azure.Data.Tables;
using Rossan.Azure.TableStarage.Attributes;
using System.Reflection;

namespace Rossan.Azure.TableStarage
{
    public class TableStoreRepository : ITableStoreRepository
    {
        protected TableServiceClient _tableServiceClient { get; set; }

        private readonly object lockObj = new object();

        private readonly Dictionary<Type, string> entityTableMapping = new Dictionary<Type, string>();

        public TableStoreRepository(string storageUri, string accountName, string storageAccountKey)
        {
            if (!string.IsNullOrWhiteSpace(storageUri) && !string.IsNullOrWhiteSpace(accountName) && !string.IsNullOrWhiteSpace(storageAccountKey))
            {
                _tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
            }
        }

        public async Task CreateAsync<T>(T data, string tableName = "") where T : ITableEntity
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            try
            {
                await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                await tableClient.UpsertEntityAsync<T>(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateEntityGroupAsync<T>(IEnumerable<T> data, string tableName = "") where T : ITableEntity
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            var groupedByPKey = data.GroupBy(t => t.PartitionKey);
            for (int i = 0; i < groupedByPKey.Count(); i++)
            {
                var group = groupedByPKey.ElementAt(i);
                foreach (var item in group)
                {
                    await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
                    var tableClient = _tableServiceClient.GetTableClient(tableName);
                    await tableClient.UpsertEntityAsync<T>(item);
                }
            }
        }

        public async Task DeleteAsync<T>(string partitionKey, string rowKey, string tableName = "") where T : ITableEntity
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.DeleteEntityAsync(partitionKey, rowKey).ConfigureAwait(false);
        }

        public async Task DeleteEntityGroupAsync<T>(IEnumerable<T> data, string tableName = "") where T : ITableEntity
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            var groupedByPKey = data.GroupBy(t => t.PartitionKey);
            for (int i = 0; i < groupedByPKey.Count(); i++)
            {
                var group = groupedByPKey.ElementAt(i);
                foreach (var item in group)
                {
                    tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
                    await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
                    var tableClient = _tableServiceClient.GetTableClient(tableName);
                    await tableClient.DeleteEntityAsync(item.PartitionKey, item.RowKey).ConfigureAwait(false);
                }
            }
        }

        public async Task<Pageable<T>> GetAsync<T>(string partitionKey, int pageSize = 0, string? continuationToken = null, string tableName = "") where T : class, ITableEntity, new()
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new NotSupportedException("PartitionKey is mandatory while making a query.");
            }
            await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            /// tableClient.Query<T>(e => e.PartitionKey == partitionKey, pageSize);
            return tableClient.Query<T>($"PartitionKey eq '{partitionKey}'", pageSize);
        }

        public async Task<T> GetItemAsync<T>(string partitionKey, string rowKey, string tableName = "") where T : class, ITableEntity, new()
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            return await tableClient.GetEntityAsync<T>(partitionKey, rowKey).ConfigureAwait(false);

        }

        public async Task UpdateAsync<T>(T data, string tableName = "") where T : class, ITableEntity, new()
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            try
            {
                await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                await tableClient.UpdateEntityAsync<T>(data, ETag.All).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateEntityGroupAsync<T>(IEnumerable<T> data, string tableName = "") where T : class, ITableEntity, new()
        {
            tableName = string.IsNullOrEmpty(tableName) ? this.ResolveTableName(typeof(T)) : tableName;
            var groupedByPKey = data.GroupBy(t => t.PartitionKey);
            for (int i = 0; i < groupedByPKey.Count(); i++)
            {
                var group = groupedByPKey.ElementAt(i);
                foreach (var item in group)
                {
                    await _tableServiceClient.CreateTableIfNotExistsAsync(tableName).ConfigureAwait(false);
                    var tableClient = _tableServiceClient.GetTableClient(tableName);
                    await tableClient.UpdateEntityAsync<T>(item, ETag.All).ConfigureAwait(false);
                }
            }
        }

        protected virtual string ResolveTableName<T>(T tableType)
           where T : Type
        {
            this.entityTableMapping.TryGetValue(tableType, out string? name);

            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            var attr = tableType.GetCustomAttribute(typeof(TableNameAttribute)) as TableNameAttribute;
            if (attr is null || string.IsNullOrEmpty(attr.TableName))
            {
                throw new NotSupportedException(
                    $"Table name is not supplied for Type : {tableType.Name}. Either use the TableNameAttribute or override the TableName property in concrete class to specify the table name.");
            }

            lock (lockObj)
            {
                if (!this.entityTableMapping.TryGetValue(tableType, out _))
                {
                    this.entityTableMapping.Add(tableType, attr.TableName);
                }
            }
            return attr.TableName;
        }
    }
}
