namespace Fitness.Application.Files
{
    using Fitness.Application.Contracts.Blob;
    using Fitness.Application.Contracts;
    using Fitness.Domain.Seedwork;
    using Fitness.Domain;
    using MediatR;
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    public sealed record DeleteFileByIdCommand(Guid FileId): IRequest;

    internal sealed class DeleteFileByIdCommandHandler : IRequestHandler<DeleteFileByIdCommand>
    {
        private readonly IBlobService blobService;
        private readonly IUnitOfWork unitOfWork;

        public DeleteFileByIdCommandHandler(IBlobService blobService, IUnitOfWork unitOfWork)
        {
            this.blobService = blobService;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteFileByIdCommand request, CancellationToken cancellationToken)
        {
            var file = await this.unitOfWork.Files.GetByIdSafeAsync(request.FileId, cancellationToken);

            var type = Enumeration.FromValue<FileType>(file.FileTypeId);

            this.unitOfWork.Files.Delete(file);

            await this.blobService.DeleteAsync(type.Name, file.Id.ToString(), cancellationToken);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
