namespace Fitness.Infrastructure.Db.Users.Repositories
{
    using Fitness.Application.Contracts;
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> refreshTokens;

        public RefreshTokenRepository(UsersDbContext context)
        {
            this.refreshTokens = context.Set<RefreshToken>();
        }

        public void Add(RefreshToken refreshToken)
        {
            this.refreshTokens.Add(refreshToken);
        }

        public void Delete(RefreshToken refreshToken)
        {
            this.refreshTokens.Remove(refreshToken);
        }

        public void Delete(List<RefreshToken> refreshTokens)
        {
            this.refreshTokens.RemoveRange(refreshTokens);
        }

        public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await this.refreshTokens.Where(rt => rt.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.refreshTokens.FirstOrDefaultAsync(rt => rt.Id == id, cancellationToken);
        }
    }
}
