namespace Fitness.Infrastructure.Db.Users
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using System;

    internal sealed class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

            optionsBuilder.UseSqlServer(null);

            var instance = new UsersDbContext(optionsBuilder.Options);

            return instance is null ? throw new InvalidOperationException($"Unable to initialize {nameof(UsersDbContext)} instance.") : instance;
        }
    }
}
