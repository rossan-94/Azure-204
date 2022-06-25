using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Rossan.Azure.BlobStore;
using Rossan.Azure.BlobStore.AzureBlobStore;
using System.Text;

namespace Azure.BlobStore
{
    public class ReadAzureBlobs
    {
        IBlobStoreRepository azureBlobRepository = new AzureBlobStoreRepository("<connectionstring>", 60);

        public async Task ReadBlobWithSasAsync(Uri sasUri)
        {
            // Try performing a read operation using the blob SAS provided.

            // Create a blob client object for blob operations.
            BlobClient blobClient = new BlobClient(sasUri, null);

            // Download and read the contents of the blob.
            try
            {
                Console.WriteLine("Blob contents:");

                // Download blob contents to a stream and read the stream.
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
                using (StreamReader reader = new StreamReader(blobDownloadInfo.Content, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Read operation succeeded for SAS {0}", sasUri);
                Console.WriteLine();
            }
            catch (RequestFailedException e)
            {
                // Check for a 403 (Forbidden) error. If the SAS is invalid, 
                // Azure Storage returns this error.
                if (e.Status == 403)
                {
                    Console.WriteLine("Read operation failed for SAS {0}", sasUri);
                    Console.WriteLine("Additional error information: " + e.Message);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                    throw;
                }
            }
        }

        public async Task GetSasTokenOfBlob(string container, string blob, string permission)
        {
            var uri = await azureBlobRepository.GetSasTokenAsync(container, blob, permission).ConfigureAwait(false);
            await ReadBlobWithSasAsync(new Uri(uri)).ConfigureAwait(false);
        }

        public async Task GetFileUriAsync(string container, string blob, string permission)
        {
            string uri = await  azureBlobRepository.GetFileUriAsync(container, blob, permission).ConfigureAwait(false);
            Console.WriteLine(uri);
        }

        public async Task ContainerExistsAsync(string container)
        {
            bool isExist = await azureBlobRepository.ContainerExistsAsync(container).ConfigureAwait(false);
            Console.WriteLine(isExist);
        }

        public async Task CreateContainerAsync(string container)
        {
            bool created = await azureBlobRepository.CreateContainerAsync(container).ConfigureAwait(false);
            Console.WriteLine(created);
        }

        public async Task UploadFileToBlobStorageAsync(string container, string blob, byte[] file, string contentType, Dictionary<string, string>? metaDataProperties)
        {
            string uri = await azureBlobRepository.UploadFileToBlobStorageAsync(container, blob, file, contentType, metaDataProperties).ConfigureAwait(false);
            Console.WriteLine(uri);
        }

        public async Task DownloadFileFromBlobStorageAsync(string container, string blob, bool isDelete)
        {
            var content = await azureBlobRepository.DownloadFileFromBlobStorageAsync(container, blob, isDelete).ConfigureAwait(false);
            Console.WriteLine(Encoding.ASCII.GetString(content.ToArray()));
        }

        public async Task DeleteFileFromContainerAsync(string container, string blob)
        {
            var isDeleted = await azureBlobRepository.DeleteFileFromContainerAsync(container, blob).ConfigureAwait(false);
            Console.WriteLine(isDeleted);
        }

        public async Task DeleteAllFilesFromContainerAsync(string container)
        {
            var isDeleted = await azureBlobRepository.DeleteAllFilesFromContainerAsync(container).ConfigureAwait(false);
            Console.WriteLine(isDeleted);
        }

        public async Task DeleteContainerAsync(string container)
        {
            var containerExists = await azureBlobRepository.DeleteContainerAsync(container).ConfigureAwait(false);
            Console.WriteLine(containerExists);
        }
    }
}
