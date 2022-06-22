using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rossan.Azure.TableStarage;

[assembly: FunctionsStartup(typeof(Azure.TableStorage.Startup))]

namespace Azure.TableStorage
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITableStoreRepository>((tsr) => { return new TableStoreRepository("https://az204storageaccountrp1.table.core.windows.net/", "az204storageaccountrp1", "PLOG/Sp5JzQnZOVr96hr4KzG+QhOctI1jrAF8vycxbNdgLvt7pHNt7der8v4Rp5tnadFZAn1sq+3+AStSAqSqQ=="); });
        }
    }
}
