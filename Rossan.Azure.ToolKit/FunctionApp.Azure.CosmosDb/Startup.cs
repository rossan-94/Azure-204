using CosmosDb.Manager;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rossan.Azure.CosmosDB;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp.Azure.CosmosDb.Startup))]

namespace FunctionApp.Azure.CosmosDb
{
    public class Startup : FunctionsStartup
    {
        string cosmosEndpointUri = Environment.GetEnvironmentVariable("cosmosEndpointUri");
        string cosmosKey = Environment.GetEnvironmentVariable("cosmosKey");
        string database = Environment.GetEnvironmentVariable("database");

        // Orders
        string orderContainerName = Environment.GetEnvironmentVariable("orderContainerName");
        string orderpartitionKey = Environment.GetEnvironmentVariable("orderPartitionKey");

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ICosmosDbRepository<Order>>((cbr) =>
            {
                var cosmosDbRepo = new CosmosDbRepository<Order>(cosmosEndpointUri, cosmosKey, database);
                return cosmosDbRepo;
            })
                .AddSingleton<OrderManager>((om) =>
                {
                    var orderManager = new OrderManager(om.GetRequiredService<ICosmosDbRepository<Order>>(), database, orderContainerName, orderpartitionKey);
                    // await orderManager.CreateConatinerAsync().ConfigureAwait(false); till now async call is not supported 
                    return orderManager;
                });
        }
    }
}
