using Azure.Identity;
using Azure.Storage.Blobs;

namespace Rossan.Azure.BlobStore.AzureBlobStore
{
    public class AzureBlobStoreRepository : BlobStoreRepository
    {
        /// <summary>
        /// AzureBlobStoreRepository
        /// </summary>
        /// <param name="blobEndpoint"></param>
        /// <param name="accountKey"></param>
        /// <param name="accountName"></param>
        /// <param name="sasexpiry"></param>
        public AzureBlobStoreRepository(string blobEndpoint, string accountKey, string accountName, int sasExpiry) : base(sasExpiry)
        {
            blobServiceClient = new BlobServiceClient(new Uri(blobEndpoint), new DefaultAzureCredential());
        }

        public AzureBlobStoreRepository(string connectionString, int sasExpiry) : base(sasExpiry)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
        } 
    }
}
