namespace Fitness.Application.Files
{
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Blob;
    using Fitness.Domain;
    using Fitness.Domain.Seedwork;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed record DownloadFileByRangeQuery(Guid FileId, long From, long To) : IRequest<DownloadFile>;

    internal sealed class DownloadFileByRangeQueryHandler : IRequestHandler<DownloadFileByRangeQuery, DownloadFile>
    {
        private const int Offset = 1024 * 1024;
        private readonly IBlobService blobService;
        private readonly IUnitOfWork unitOfWork;

        public DownloadFileByRangeQueryHandler(IBlobService blobService, IUnitOfWork unitOfWork)
        {
            this.blobService = blobService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<DownloadFile> Handle(DownloadFileByRangeQuery request, CancellationToken cancellationToken)
        {
            var file = await this.unitOfWork.Files.GetByIdSafeAsync(request.FileId, cancellationToken);

            var type = Enumeration.FromValue<FileType>(file.FileTypeId);

            var from = request.From;

            var to = request.To == 0 ? from + Offset : request.To;

            if (to > file.FileLength)
            {
                to = file.FileLength;
            }

            return await this.blobService.GetAsync(
                type.Name,
                file.Id.ToString(),
                from,
                to,
                file.FileLength,
                cancellationToken);
        }
    }
}
