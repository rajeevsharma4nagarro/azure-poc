using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using SCD.Services.ProductAPI.Utility.IUtility;

namespace SCD.Services.ProductAPI.Utility
{
    public class UploadFilesToBlob: IUploadFilesToBlob
    {
        private readonly IConfiguration _configuration;
        public UploadFilesToBlob(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadToBlob(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            string? connectionString = _configuration.GetValue<string>("StorageAccount:ConnectionString");
            string? containerName = _configuration.GetSection("StorageAccount:ContainerName").Value;

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(filename);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
