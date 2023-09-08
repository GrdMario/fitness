namespace Fitness.Presentation.Api.Controllers.V1
{
    using Fitness.Application.Files;
    using Fitness.Presentation.Api.Internal.Constants;
    using Fitness.Presentation.Api.Internal.Extensions;
    using Fitness.Presentation.Api.Internal.Models.ActionResults;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiVersion(ApiVersions.V1)]
    internal sealed class FileController : ApiControllerBase
    {
        public FileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            return await this.ProcessAsync(new DeleteFileByIdCommand(id));
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadAsync(Guid id)
        {
            var file = await this.Mediator.Send(new DownloadFileByIdQuery(id));

            return this.File(file.Data, file.ContentType);
        }

        [HttpGet("{id}/download/range")]
        public async Task<IActionResult> StreamAsync(Guid id)
        {
            var range = this.HttpContext.Request.ParseContentLenghtHeader();

            var file = await this.Mediator.Send(new DownloadFileByRangeQuery(id, range.From, range.To));

            return new VideoStreamResult(file);
        }

        [HttpPost("upload/{type}")]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public async Task<IActionResult> UploadAsync(IFormFile file, string type, CancellationToken cancellationToken)
        {
            var command = await file.AsUploadFileCommand(type, cancellationToken);

            await this.Mediator.Send(command, cancellationToken);

            return this.NoContent();
        }
    }
}
