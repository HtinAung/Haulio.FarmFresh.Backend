using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Shared
{
    public class BlobStorageHelper
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageHelper(
            BlobContainerClient blobContainerClient
        )
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<string> UploadFile(Stream stream, string blobName)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }
    }
}
