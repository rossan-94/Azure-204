using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rossan.Azure.KeyVault;
using Rossan.Azure.KeyVault.Manager;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp.Azure.KeyVault.Startup))]

namespace FunctionApp.Azure.KeyVault
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string dbsName = Environment.GetEnvironmentVariable("dbsName");

            builder.Services.AddSingleton<IKeyVaultRepository>((sp) =>
            {
                return new KeyVaultManager(dbsName);
            });
        }
    }
}
