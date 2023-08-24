namespace Fitness.Application.Authentication
{
    using Fitness.Application.Authentication.Validators;
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Security;
    using Fitness.Blocks.Common.Kernel;
    using Fitness.Domain;
    using FluentValidation;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public record RegisterCommand(string Email, string Password) : IRequest;

    internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public RegisterCommandValidator(IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            this.unitOfWork = unitOfWork;

            this.RuleFor(r => r.Email)
                .EmailAddress();

            this.RuleFor(r => r.Email)
                .MustAsync(this.HasUser)
                .WithMessage("Email in use.");

            this.RuleFor(r => r.Password)
                .SetValidator(new PasswordValidator(passwordService));
        }

        private async Task<bool> HasUser(string email, CancellationToken cancellationToken)
        {
            var user = await this.unitOfWork.Users.GetByEmailAsync(email, cancellationToken);

            return user is null;
        }
    }

    internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHashService passwordHashService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IPasswordHashService passwordHashService)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHashService = passwordHashService;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userId = SystemGuid.NewGuid;

            var emailVerificationCode = SystemGuid.NewGuid.ToString();

            var emailVerificationCodeExpirationDate = SystemClock.UtcNow.AddMinutes(20);

            var password = this.passwordHashService.Hash(request.Password);

            var profile = new Profile(userId, null, null, null);

            var emailVerification = new EmailVerification(false, emailVerificationCode, emailVerificationCodeExpirationDate);

            var claim = new Claim(SystemGuid.NewGuid, userId, UserClaims.User);

            var user = new User(
                userId,
                request.Email,
                password,
                profile,
                emailVerification,
                new List<Claim>() { claim });

            this.unitOfWork.Users.Create(user);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
