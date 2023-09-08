namespace Fitness.Presentation.Api.Internal.Models.ActionResults
{
    using Fitness.Application.Contracts.Blob;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using System.IO;
    using System.IO.Pipelines;
    using System.Threading;
    using System.Threading.Tasks;

    public class VideoStreamResult : ActionResult
    {
        const string CacheControl = "public, max-age=604800";
        const string AcceptRanges = "bytes";
        private readonly int statusCode = 206;
        private readonly DownloadFile file;

        public VideoStreamResult(DownloadFile file)
        {
            this.file = file;
        }

        override public async Task ExecuteResultAsync(ActionContext context)
        {

            var response = context.HttpContext.Response;
            response.StatusCode = statusCode;
            response.Headers.CacheControl = CacheControl;
            response.Headers.AcceptRanges = AcceptRanges;
            response.Headers.ContentType = file.ContentType;
            response.Headers.ContentLength = file.Length;

            if (statusCode == StatusCodes.Status206PartialContent)
            {
                response.Headers.ContentRange = new ContentRangeHeaderValue(file.From, file.To, file.Length).ToString();
            }

            // Be sure to cancel flowing the stream if the request is aborted
            CancellationToken ct = context.HttpContext.RequestAborted;

            using var stream = new MemoryStream(file.Data);

            await stream.CopyToAsync(response.BodyWriter, ct);
        }
    }
}
