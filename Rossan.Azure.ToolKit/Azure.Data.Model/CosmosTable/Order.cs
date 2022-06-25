using Newtonsoft.Json;

namespace Azure.Data.Model.CosmosTable
{
    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; } = Guid.NewGuid().ToString(); // Required field for every item on cosmos db if adding by program

        public string? orderId { get; set; }
        public string? category { get; set; }
        public string? quantity { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
