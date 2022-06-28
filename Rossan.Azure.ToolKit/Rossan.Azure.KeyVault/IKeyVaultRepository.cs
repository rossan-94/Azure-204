using Azure.Security.KeyVault.Secrets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rossan.Azure.KeyVault
{
    public interface IKeyVaultRepository
    {
        Task<KeyVaultSecret> GetSecretAsync(string secretName);
    }
}
