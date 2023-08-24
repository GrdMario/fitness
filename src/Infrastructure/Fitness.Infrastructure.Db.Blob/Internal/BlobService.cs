namespace Fitness.Infrastructure.Db.Blob.Internal
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Fitness.Application.Contracts.Blob;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal sealed class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public Task<DownloadFile> AddAsync(UploadFile file, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Get(CancellationToken cancellationToken)
        {
            var cs = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            string blobUrl = "http://127.0.0.1:10000/devstoreaccount1/fitness-thumbnail/test.xml";

            var blobServiceClient = new BlobServiceClient(cs);
            var bub = new BlobUriBuilder(new Uri(blobUrl));
            var containerClient = this.blobServiceClient.GetBlobContainerClient(bub.BlobContainerName);
            var client = containerClient.GetBlobClient(bub.BlobName);

            var response = await client.GetPropertiesAsync(cancellationToken: cancellationToken);
            var properties = response.Value;
            long contentLength = properties.ContentLength;
            DateTimeOffset lastModified = properties.LastModified;



            var x = blobServiceClient.GetBlobContainerClient("fitness-thumbnail");
        }

        public void Get()
        {

            throw new NotImplementedException();
        }

        public async Task<DownloadFile> GetFileAsync(string containerName, string blobName, long From, long? To, CancellationToken cancellationToken)
        {
            var containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);

            var client = containerClient.GetBlobClient(blobName);

            using var memoryStream = new MemoryStream();

            var options = new BlobDownloadOptions()
            {
                Range = new Azure.HttpRange(From, To)
            };

            var result = await client.DownloadContentAsync(options, cancellationToken);

            if (result.GetRawResponse().IsError)
            {
                throw new ApplicationException("Error while downloading file.");
            }

            throw new NotImplementedException();
        }

        public async Task GetPropetiesAsync(string blobName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class BlobSettings
    {
        public const string Key = nameof(BlobSettings);

        public string Url { get; set; } = default!;
    }
}
