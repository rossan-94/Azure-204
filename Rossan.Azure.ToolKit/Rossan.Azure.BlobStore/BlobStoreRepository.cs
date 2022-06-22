using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

namespace Rossan.Azure.BlobStore
{
    /// <summary>
    /// Blob Store Repository
    /// </summary>
    public abstract class BlobStoreRepository : IBlobStoreRepository
    {
        /// <inheritdoc/>
        public BlobServiceClient blobServiceClient { get; set; }

        /// <inheritdoc />
        public int SasExpiryTimeout { get; set; }

        /// <inheritdoc />
        public string BaseUrl
        {
            get
            {
                return blobServiceClient?.Uri?.AbsoluteUri;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStoreRepository"/> class.
        /// </summary>
        /// <param name="sasexpiry">SAS token expiry</param>
        protected BlobStoreRepository(int sasexpiry)
        {
            SasExpiryTimeout = sasexpiry;
        }

        /// <summary>
        /// Get the SAS Token
        /// </summary>
        /// <param name="containername">Name of the Container</param>
        /// <param name="fileName">fileName</param>
        /// <param name="permissions">permissions for the storage account</param>
        /// <returns>Returns the SAS Token for the file or Container</returns>
        public async Task<string> GetSasTokenAsync(string containername, string fileName, string permissions, string? storePolicyName = null)
        {
            string sasUri = string.Empty;
            // Build sas  
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder();

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containername);
            var exists = await blobContainerClient.ExistsAsync().ConfigureAwait(false);
            if (!exists)
            {
                throw new Exception($"Container Does not exists : {containername}");
            }

            if (!string.IsNullOrWhiteSpace(storePolicyName))
            {
                blobSasBuilder.Identifier = storePolicyName;
            }
            else
            {
                blobSasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(SasExpiryTimeout);
                blobSasBuilder.SetPermissions(PermissionBuilder.GetPermissions(permissions));
                blobSasBuilder.BlobContainerName = containername;
            }


            if (fileName == null)
            {
                if (blobContainerClient.CanGenerateSasUri)
                {
                    blobSasBuilder.Resource = "b";                  
                }
                sasUri = blobContainerClient.GenerateSasUri(blobSasBuilder).ToString();
            }
            else
            {
               var blobClient = blobContainerClient.GetBlobClient(fileName);
                if (blobClient.CanGenerateSasUri)
                {
                    blobSasBuilder.Resource = "c";
                    blobSasBuilder.BlobName = fileName;                        
                }
                sasUri = blobClient.GenerateSasUri(blobSasBuilder).ToString();
            }
            return sasUri;
        }

        /// <inheritdoc/>
        public async Task<string> GetFileUriAsync(string containerName, string fileName, string permissions)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var exists = await blobContainerClient.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                throw new Exception($"Container Does not exists : {containerName}");
            }

            var blobClient = blobContainerClient.GetBlobClient(fileName);

            string sasToken = await GetSasTokenAsync(containerName, fileName, permissions).ConfigureAwait(false);
            return blobClient.Uri.AbsoluteUri; // + sasToken;
        }

        public async Task<bool> ContainerExistsAsync(string containerName)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return await blobContainerClient.ExistsAsync().ConfigureAwait(false);
        }

        public async Task<bool> CreateContainerAsync(string containerName)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeleteAllFilesFromContainerAsync(string containerName)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await foreach (BlobItem blob in blobContainerClient.GetBlobsAsync())
            {
                blobContainerClient.DeleteBlob(blob.Name);
            }

            return true;
        }

        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return await blobContainerClient.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteFileFromContainerAsync(string containerName, string fileName)
        {

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        public async Task<MemoryStream> DownloadFileFromBlobStorageAsync(string containerName, string fileName, bool delete = false)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream).ConfigureAwait(false);

            if (delete)
            {
                await blobClient.DeleteAsync().ConfigureAwait(false);
            }

            return memoryStream;
        }

        public async Task<string> UploadFileToBlobStorageAsync(string containerName, string filename, byte[] file, string contentType, Dictionary<string, string>? metaDataProperties = null)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blobClient = blobContainerClient.GetBlobClient(filename);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            };

            if (metaDataProperties != null && metaDataProperties.Any())
            {
                await blobClient.SetMetadataAsync(metaDataProperties).ConfigureAwait(false);
            }
            await blobClient.UploadAsync(new MemoryStream(file), blobHttpHeaders).ConfigureAwait(false);

            return blobClient.Uri.ToString();
        }

        public Task AppendToBlobAsync(string containerName, string fileName, string content)
        {
            throw new NotImplementedException();
        }
    }
}
