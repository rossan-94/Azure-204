using Azure.Data.Tables;

namespace Azure.Data.Model.AzureTable
{
    public class Order : ITableEntity
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string? Name { get; set; }

        public int Quantity { get; set; }
    }
}
