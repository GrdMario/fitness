namespace Fitness.Application.Contracts.Blob
{
    using Fitness.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBlobService
    {
        Task<DownloadFile> GetAsync(
            string containerName,
            string blobName,
            long start,
            long end,
            long lenght,
            CancellationToken cancellationToken);

        Task UploadAsync(string containerName, string blobName, byte[] data, CancellationToken cancellationToken);

        Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken);
    }

    public record DownloadFile(long From, long To, byte[] Data, long Length, string ContentType);
}
