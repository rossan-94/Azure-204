namespace Azure.Data.Model.CosmosTable
{
    public class CreateContainer
    {
        public string? ContainerName { get; set; }
        public string? PartitionKey { get; set; }
        public string? Database { get; set; }
    }
}
