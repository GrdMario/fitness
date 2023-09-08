namespace Fitness.Presentation.Api.Internal.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System.Linq;

    // TODO: Move this logic to http context accessor?
    internal static class HttpRequestExtensions
    {
        public record Range(long From, long To);

        /// <summary>
        /// Extracts <see cref="Range"/> from headers.
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/>.</param>
        /// <returns>Instance of <see cref="Range"/>.</returns>
        public static Range ParseContentLenghtHeader(this HttpRequest request)
        {
            var range = request.GetTypedHeaders().Range;

            if (range == null)
            {
                return new(0, 0);
            }

            var result = range.Ranges
                .Select(s =>
                    new Range(
                        s.From ?? 0,
                        s.To ?? 0))
                .FirstOrDefault();

            if (result == null)
            {
                return new(0, 0);
            }

            return result;
        }
    }
}
