namespace Fitness.Application.Files
{
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Blob;
    using Fitness.Domain.Seedwork;
    using Fitness.Domain;
    using MediatR;
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    public sealed record DownloadFileByIdQuery(Guid FileId) : IRequest<DownloadFile>;

    internal sealed class DownloadFileByIdQueryHandler : IRequestHandler<DownloadFileByIdQuery, DownloadFile>
    {
        private readonly IBlobService blobService;
        private readonly IUnitOfWork unitOfWork;

        public DownloadFileByIdQueryHandler(IBlobService blobService, IUnitOfWork unitOfWork)
        {
            this.blobService = blobService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<DownloadFile> Handle(DownloadFileByIdQuery request, CancellationToken cancellationToken)
        {
            var file = await this.unitOfWork.Files.GetByIdSafeAsync(request.FileId, cancellationToken);

            var type = Enumeration.FromValue<FileType>(file.FileTypeId);

            return await this.blobService.GetAsync(
                type.Name,
                file.Id.ToString(),
                0,
                file.FileLength,
                file.FileLength,
                cancellationToken);
        }
    }
}
