// See https://aka.ms/new-console-template for more information
using Azure.BlobStore;

string container = "data";
string blob = "9789389269734.pdf";
string permission = "r";

string newContainer = "data1";

// byte[] file = File.ReadAllBytes("resouncedata/BICCO_2022_09762.pdf");

var readAzureBlobs = new ReadAzureBlobs();
// await readAzureBlobs.GetSasTokenOfBlob(newContainer, blob, permission).ConfigureAwait(false);

// await readAzureBlobs.GetFileUriAsync(container, blob, permission).ConfigureAwait(false);

// await readAzureBlobs.ContainerExistsAsync(container).ConfigureAwait(false);

// await readAzureBlobs.CreateContainerAsync(newContainer).ConfigureAwait(false);

// await readAzureBlobs.UploadFileToBlobStorageAsync(newContainer, blob, file, "application/json", null).ConfigureAwait(false);

// await readAzureBlobs.DownloadFileFromBlobStorageAsync(newContainer, blob, false).ConfigureAwait(false);

// await readAzureBlobs.DownloadFileFromBlobStorageAsync(newContainer, "97893892697341.pdf", false).ConfigureAwait(false);

// await readAzureBlobs.DeleteFileFromContainerAsync(container, blob).ConfigureAwait(false);

// await readAzureBlobs.DeleteAllFilesFromContainerAsync(newContainer).ConfigureAwait(false);

await readAzureBlobs.DeleteContainerAsync(newContainer).ConfigureAwait(false);


Console.WriteLine("Hello, World!");
