namespace Fitness.Application.Authentication
{
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Email;
    using Fitness.Application.Contracts.Security;
    using Fitness.Domain;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public record ResetPasswordCommand(string Email) : IRequest;

    internal sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator() {

            this.RuleFor(r => r.Email).EmailAddress();
        }
    }

    internal sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordService passwordService;
        private readonly IPasswordHashService passwordHashService;
        private readonly IEmailService emailService;

        public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordService passwordService, IPasswordHashService passwordHashService, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.passwordService = passwordService;
            this.passwordHashService = passwordHashService;
            this.emailService = emailService;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            User user = await this.unitOfWork.Users.GetByEmailSafeAsync(request.Email, cancellationToken);

            var password = this.passwordService.Generate();

            var hash = this.passwordHashService.Hash(password);

            user.SetPassword(hash);

            this.unitOfWork.Users.Update(user);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            await this.emailService.SendPasswordResetEmailAsync(user);
        }
    }
}
