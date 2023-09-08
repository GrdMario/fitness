namespace Fitness.Presentation.Api.Internal.Extensions
{
    using Fitness.Application.Files;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class FormFileExtensions
    {
        public static async Task<UploadFileCommand> AsUploadFileCommand(
            this IFormFile file,
            string type,
            CancellationToken cancellationToken)
        {
            if (file is null)
            {
                throw new ApplicationException("Unable to resolve file");
            }

            string name = WebUtility.HtmlEncode(file.FileName) ?? string.Empty;

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            using var stream = new MemoryStream();

            await file.CopyToAsync(stream, cancellationToken);

            var bytes = stream.ToArray();

            var result = new UploadFileCommand(name, bytes, extension, file.ContentDisposition, type);

            return result;
        }
    }
}
