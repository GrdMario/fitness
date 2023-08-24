namespace Fitness.Application.Authentication
{
    using Fitness.Application.Authentication.Models;
    using Fitness.Application.Authentication.Validators;
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Security;
    using Fitness.Blocks.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public record LoginCommand(string Email, string Password) : IRequest<TokenResponse>;

    internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator(IPasswordService passwordService)
        {
            this.RuleFor(r => r.Email).EmailAddress();

            this.RuleFor(r => r.Password).SetValidator(new PasswordValidator(passwordService));
        }
    }

    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHashService passwordHashService;
        private readonly ITokenService tokenService;
        public LoginCommandHandler(IPasswordHashService passwordHashService, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            this.passwordHashService = passwordHashService;
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
        }

        public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await this.unitOfWork.Users.GetByEmailSafeAsync(request.Email, cancellationToken);

            if (!user.EmailVerification.IsVerified)
            {
                throw new ServiceValidationException("Invalid credentials.");
            }

            var password = this.passwordHashService.VerifyHash(user.Password, request.Password);

            if (!password)
            {
                throw new ServiceValidationException("Invalid credentials.");
            }

            var refreshToken = this.tokenService.GenerateRefreshToken(user);

            var jwtToken = this.tokenService.GenerateAccessToken(user);

            var oldTokens = await this.unitOfWork.RefreshTokens.GetByUserIdAsync(user.Id, cancellationToken);

            this.unitOfWork.RefreshTokens.Delete(oldTokens);

            this.unitOfWork.RefreshTokens.Add(refreshToken);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            return new TokenResponse(
                jwtToken,
                refreshToken.Id.ToString()
            );
        }
    }
}
