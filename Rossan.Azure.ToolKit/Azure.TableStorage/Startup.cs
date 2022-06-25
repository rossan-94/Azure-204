using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rossan.Azure.TableStarage;
using System;

[assembly: FunctionsStartup(typeof(Azure.TableStorage.Startup))]

namespace Azure.TableStorage
{
    public class Startup : FunctionsStartup
    {
        string storageUri = Environment.GetEnvironmentVariable("storageUri");
        string accountName = Environment.GetEnvironmentVariable("accountName");
        string accountKey = Environment.GetEnvironmentVariable("accountKey");

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITableStoreRepository>((tsr) => { return new TableStoreRepository(storageUri, accountName, accountKey); });
        }
    }
}
