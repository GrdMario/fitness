namespace Fitness.Application.Authentication
{
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Email;
    using Fitness.Blocks.Common.Kernel;
    using Fitness.Domain;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public record ResendEmailVerificationCommand(string Email) : IRequest;

    internal sealed class ResendEmailVerificationCommandValidator : AbstractValidator<ResendEmailVerificationCommand>
    {
        public ResendEmailVerificationCommandValidator()
        {
            this.RuleFor(r => r.Email).EmailAddress();
        }
    }

    internal sealed class ResendEmailVerificationCommandHandler : IRequestHandler<ResendEmailVerificationCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;

        public ResendEmailVerificationCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }

        public async Task Handle(ResendEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            User? user = await this.unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return;
            }

            if (user.EmailVerification.IsVerified)
            {
                return;
            }

            var emailVerification = new EmailVerification(
                false,
                SystemGuid.NewGuid.ToString(),
                SystemClock.UtcNow.AddMinutes(20));

            user.AddEmailVerification(emailVerification);

            this.unitOfWork.Users.Update(user);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            await this.emailService.SendEmailVerificationAsync(user);
        }
    }
}
