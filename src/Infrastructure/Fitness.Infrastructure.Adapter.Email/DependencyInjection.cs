namespace Fitness.Infrastructure.Adapter.Email
{
    using Fitness.Application.Contracts.Email;
    using Fitness.Infrastructure.Adapter.Email.Internal;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddEmailAdapter(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();

            return services;

        }
    }
}