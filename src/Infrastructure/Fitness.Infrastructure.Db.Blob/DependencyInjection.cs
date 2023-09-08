namespace Fitness.Infrastructure.Db.Blob
{
    using Fitness.Application.Contracts.Blob;
    using Fitness.Infrastructure.Db.Blob.Internal;
    using Microsoft.Extensions.Azure;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddBlobLayer(this IServiceCollection services, BlobSettings settings)
        {

            services.AddAzureClients(cb =>
            {
                cb.AddBlobServiceClient(settings.Url);

            });

            services.AddScoped<IBlobService, BlobService>();

            return services;
        }
    }

    public class BlobSettings
    {
        public const string Key = nameof(BlobSettings);

        public string Url { get; set; } = default!;
    }
}