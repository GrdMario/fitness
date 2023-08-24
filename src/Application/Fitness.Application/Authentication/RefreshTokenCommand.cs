namespace Fitness.Application.Authentication
{
    using Fitness.Application.Authentication.Models;
    using Fitness.Application.Authentication.Validators;
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Security;
    using Fitness.Blocks.Common.Exceptions;
    using Fitness.Blocks.Common.Kernel;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public record RefreshTokenCommand(Guid RefreshToken) : IRequest<TokenResponse>;

    internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
        }

        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken =
                await this.unitOfWork.RefreshTokens.GetByIdAsync(request.RefreshToken, cancellationToken)
                ?? throw new ServiceAuthorizationException("Unathorized.");

            if (refreshToken.ExpiresAt < SystemClock.UtcNow)
            {
                throw new ServiceAuthorizationException("Unauthorized.");
            }

            var user = await this.unitOfWork.Users.GetByIdSafeAsync(refreshToken.UserId, cancellationToken);

            var token = this.tokenService.GenerateAccessToken(user);

            var rc = this.tokenService.GenerateRefreshToken(user);

            // TODO: Consider using update instead.
            this.unitOfWork.RefreshTokens.Add(rc);

            this.unitOfWork.RefreshTokens.Delete(refreshToken);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            return new TokenResponse(token, rc.Id.ToString());
        }
    }
}
