using Azure.Storage.Blobs;

namespace Rossan.Azure.BlobStore
{
    public interface IBlobStoreRepository
    {
        /// <summary>
        /// Gets or sets the blob service client
        /// </summary>
        BlobServiceClient blobServiceClient { get; set; }

        /// <summary>
        /// Get the SAS Token
        /// </summary>
        /// <param name="containername">Name of the Container</param>
        /// <param name="fileName">fileName</param>
        /// <param name="permissions">permissions for the storage account</param>
        /// <returns>Returns the SAS Token for the file or Container</returns>
        Task<string> GetSasTokenAsync(string containername, string fileName, string permissions, string? storedPolicyName = null);

        /// <summary>
        /// Get the url of the file with the SAS Token attached.
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="permissions">Permissions required on the Url</param>
        /// <returns>A string representing the file Uri</returns>
        Task<string> GetFileUriAsync(string containerName, string fileName, string permissions);

        /// <summary>
        /// Upload a file to the Blob
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <param name="filename">Name of the file</param>
        /// <param name="file">File Content</param>
        /// <param name="contentType">Content Type of the file</param>
        /// <param name="metaDataProperties">Meta data Properties on the file</param>
        /// <returns>A string representing the file uri int he blob storage</returns>
        Task<string> UploadFileToBlobStorageAsync(string containerName, string filename, byte[] file, string contentType, Dictionary<string, string> metaDataProperties = null);

        /// <summary>
        /// Appends context to blob
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="content">Content to appended</param>
        /// <returns>Nothing</returns>
        Task AppendToBlobAsync(string containerName, string fileName, string content);

        /// <summary>
        /// Download File from Blob
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="delete">If file has to be deleted?</param>
        /// <returns>A MemoryStream object representing the file from the blob storage</returns>
        Task<MemoryStream> DownloadFileFromBlobStorageAsync(string containerName, string fileName, bool delete = false);

        /// <summary>
        /// Delete a file from container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="fileName">File name</param>
        /// <returns>A bool representing success/failure of the operation</returns>
        Task<bool> DeleteFileFromContainerAsync(string containerName, string fileName);

        /// <summary>
        /// Delete All files from Container
        /// </summary>
        /// <param name="containerName">Name of the container</param>
        /// <returns>A bool representing success/failure of the operation</returns>
        Task<bool> DeleteAllFilesFromContainerAsync(string containerName);

        /// <summary>
        /// Create a container
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <returns>A bool representing success/failure of the operation</returns>
        Task<bool> CreateContainerAsync(string containerName);

        /// <summary>
        /// Delete the Container
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <returns>A bool representing success/failure of the operation</returns>
        Task<bool> DeleteContainerAsync(string containerName);

        /// <summary>
        /// Checks if container exists in blob or not.
        /// </summary>
        /// <param name="containerName">Name of the Container</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<bool> ContainerExistsAsync(string containerName);
    }
}
