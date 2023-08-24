namespace Fitness.Infrastructure.Db.Blob
{
    using Fitness.Application.Contracts.Blob;
    using Fitness.Infrastructure.Db.Blob.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddBlobLayer(this IServiceCollection services)
        {
            return services;
        }
    }
}