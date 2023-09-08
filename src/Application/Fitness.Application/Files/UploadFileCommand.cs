namespace Fitness.Application.Files
{
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Blob;
    using Fitness.Blocks.Common.Kernel;
    using Fitness.Domain;
    using Fitness.Domain.Seedwork;
    using FluentValidation;
    using MediatR;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using File = Domain.File;

    public sealed record UploadFileCommand(string Name, byte[] Data, string Extension, string ContentDisposition, string Type) : IRequest;

    internal sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        private const int AllowedFileSize = 500 * 1024 * 1024;
        private const string FormData = "form-data";

        public UploadFileCommandValidator()
        {
            this.RuleFor(r => r.Name)
                .NotEmpty();

            this.RuleFor(r => r.ContentDisposition)
                .NotEmpty()
                .Must(IsContentDispositionFormData)
                .WithMessage(model => $"Content-Disposition: '{model.ContentDisposition}' is invalid.");

            this.RuleFor(r => r.Extension)
                .NotEmpty()
                .Must((model, _) => IsValidFileExtension(model))
                .WithMessage(model => $"Extension: '{model.Extension}' is not supported.");

            this.RuleFor(r => r.Data)
                .NotEmpty()
                .Must(IsValidFileLenght)
                .WithMessage("File is to large for upload.");

            this.RuleFor(r => r.Type)
                .NotEmpty()
                .Must(IsValidFileType);
        }

        /// <summary>
        /// Checks if we support upload of those types.
        /// </summary>
        /// <param name="type">Type of a file</param>
        /// <returns>True if we supprot uploading of that file.</returns>
        private bool IsValidFileType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            var fileType = Enumeration.FromDisplayName<FileType>(type);

            return fileType is not null;

        }

        /// <summary>
        /// Checks if content disposition contains form-data.
        /// </summary>
        /// <param name="contentDisposition">Content disposition</param>
        /// <returns>True if correct content disposition is provided.</returns>
        private bool IsContentDispositionFormData(string contentDisposition)
        {
            if (string.IsNullOrWhiteSpace(contentDisposition))
            {
                return false;
            }

            return contentDisposition.Contains(FormData);
        }

        /// <summary>
        /// Checks file size
        /// </summary>
        /// <param name="data">File data.</param>
        /// <returns>True if file is not larger than 3 MB</returns>
        private static bool IsValidFileLenght(byte[] data) => data.Length > 0 && data.Length < AllowedFileSize;

        /// <summary>
        /// Checks files extension and file signature.
        /// </summary>
        /// <param name="model">Model for file upload.</param>
        /// <returns>True when file extension is allowed and file signature is correct.</returns>
        private static bool IsValidFileExtension(UploadFileCommand model)
        {
            if (string.IsNullOrEmpty(model.Extension))
            {
                return false;
            }

            var currentExtension = Enumeration.FromDisplayName<FileExtension>(model.Extension);

            if (currentExtension is null)
            {
                return false;
            }

            using var stream = new MemoryStream(model.Data);
            using var reader = new BinaryReader(stream);
            var signatures = currentExtension.Signatures;

            if (signatures.Count == 0)
            {
                return true;
            }

            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            return signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }

    internal sealed class UploadFileCommandHandler : IRequestHandler<UploadFileCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBlobService blobService;

        public UploadFileCommandHandler(IUnitOfWork unitOfWork, IBlobService blobService)
        {
            this.unitOfWork = unitOfWork;
            this.blobService = blobService;
        }

        public async Task Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var extensions = Enumeration.FromDisplayName<FileExtension>(request.Extension);
            var type = Enumeration.FromDisplayName<FileType>(request.Type);

            var file = new File(
                SystemGuid.NewGuid,
                request.Name,
                extensions,
                request.Data.Length,
                type,
                SystemGuid.NewGuid,
                false,
                SystemClock.UtcNow
            );

            this.unitOfWork.Files.Add(file);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            await this.blobService.UploadAsync(
                type.Name,
                file.Id.ToString(),
                request.Data,
            cancellationToken);
        }
    }
}
