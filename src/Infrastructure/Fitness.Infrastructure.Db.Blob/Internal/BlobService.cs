namespace Fitness.Infrastructure.Db.Blob.Internal
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Fitness.Application.Contracts.Blob;
    using System.Threading;

    internal sealed class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken)
        {
            var client = this.blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);

            await client.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public async Task<DownloadFile> GetAsync(
            string blobContainerName,
            string blobName,
            long start,
            long end,
            long lenght,
            CancellationToken cancellationToken)
        {
            var client = this.blobServiceClient.GetBlobContainerClient(blobContainerName).GetBlobClient(blobName);

            var response = await client.DownloadStreamingAsync(
                new BlobDownloadOptions()
                {
                    Range = new Azure.HttpRange(start, end)
                },
                cancellationToken
            );

            using var stream = new MemoryStream();
            await response.Value.Content.CopyToAsync(stream, cancellationToken);

            var file = new DownloadFile(
                start,
                end,
                stream.ToArray(),
                lenght,
                response.Value.Details.ContentType
            );

            return file;

        }

        public async Task UploadAsync(string containerName, string blobName, byte[] data, CancellationToken cancellationToken)
        {
            var client = this.blobServiceClient.GetBlobContainerClient(containerName);

            using var stream = new MemoryStream(data);

            await client.UploadBlobAsync(blobName, stream, cancellationToken);
        }
    }
}
