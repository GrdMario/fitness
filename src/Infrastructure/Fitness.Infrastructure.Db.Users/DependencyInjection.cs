namespace Fitness.Infrastructure.Db.Users
{
    using Fitness.Application.Contracts;
    using Fitness.Domain;
    using Fitness.Domain.Seedwork;
    using Fitness.Infrastructure.Db.Users.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureUsersConfiguration(this IServiceCollection services, MssqlSettings settings)
        {
            services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(settings.ConnectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IApplicationBuilder MigrateMssqlDb(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetService<UsersDbContext>();

            if (dbContext is null)
            {
                throw new ApplicationException(nameof(dbContext));
            }

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            return builder;
        }
    }

    public class MssqlSettings
    {
        public const string Key = nameof(MssqlSettings);
        public string? ConnectionString { get; set; } = default;
    }
}
