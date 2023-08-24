namespace Fitness.Application.Contracts
{
    using Fitness.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        void Add(RefreshToken refreshToken);

        void Delete(RefreshToken refreshToken);

        void Delete(List<RefreshToken> refreshTokens);

        Task<List<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
