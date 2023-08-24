namespace Fitness.Infrastructure.Db.Users
{
    using Fitness.Application.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly UsersDbContext context;

        public UnitOfWork(
            IUserRepository users,
            IRefreshTokenRepository refreshTokens,
            UsersDbContext context)
        {
            this.context = context;
            this.Users = users;
            this.RefreshTokens = refreshTokens;
        }
        public IUserRepository Users { get; }

        public IRefreshTokenRepository RefreshTokens { get; }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
