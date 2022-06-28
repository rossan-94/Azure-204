using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

namespace Rossan.Azure.KeyVault
{
    public class KeyVaultRepository : IKeyVaultRepository
    {

        private string DnsName { get; set; }
        protected SecretClient _secretClient;

        public KeyVaultRepository(string dnsName)
        {
            if (string.IsNullOrEmpty(dnsName))
            {
                throw new ArgumentNullException($"{nameof(dnsName)} cannot be null or blank");
            }
            DnsName = dnsName;
        }

        public async virtual Task<KeyVaultSecret> GetSecretAsync(string secretName)
        {
            return await _secretClient.GetSecretAsync(secretName);
        }
    }
}
