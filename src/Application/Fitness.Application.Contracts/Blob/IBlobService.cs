namespace Fitness.Application.Contracts.Blob
{
    using Fitness.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBlobService
    {
        //void Get();

        //Task GetPropetiesAsync(string blobName, CancellationToken cancellationToken);

        //Task<DownloadFile> GetFileAsync(long From, long To, long Size, CancellationToken cancellationToken);

        //Task<DownloadFile> AddAsync(UploadFile file, CancellationToken cancellationToken);
    }

    public record DownloadFile(long From, long To, byte[] Data);

    public record UploadFile(long From, long To, long Size, string BlobName);
}
