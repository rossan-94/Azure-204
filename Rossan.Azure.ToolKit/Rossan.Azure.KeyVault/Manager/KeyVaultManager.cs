using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace Rossan.Azure.KeyVault.Manager
{
    public class KeyVaultManager : KeyVaultRepository
    {
        // MSI    
        public KeyVaultManager(string dnsName) : base(dnsName)
        {
            _secretClient = new SecretClient(vaultUri: new Uri(dnsName), new DefaultAzureCredential());
        }

        // Application Object
        public KeyVaultManager(string dnsName, string clientId, string clientSecret, string tenantId) : base(dnsName)
        {
            _secretClient = new SecretClient(vaultUri: new Uri(dnsName), new ClientSecretCredential(tenantId, clientId, clientSecret));
        }
    }
}
