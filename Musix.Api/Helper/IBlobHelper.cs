using Azure.Storage.Blobs;

namespace Musix.Api.Helper
{
    public interface IBlobHelper
    {
        Task<string> FileUpload(string blobStorageConnectionString, string storageContainerName, IFormFile? image);
    }
    public class AzureBlobHelper : IBlobHelper
    {
        public async Task<string> FileUpload(string blobStorageConnectionString, string storageContainerName, IFormFile? image)
        {
            if (image == null)
                return String.Empty;

            BlobContainerClient blobContainerClient = new BlobContainerClient(blobStorageConnectionString, storageContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(image.FileName);
            var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            ms.Position = 0;
            await blobClient.UploadAsync(ms);

            return blobClient.Uri.AbsoluteUri;
        }
    }
}
