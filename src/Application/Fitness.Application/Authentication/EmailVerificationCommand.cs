namespace Fitness.Application.Authentication
{
    using Fitness.Application.Contracts;
    using Fitness.Blocks.Common.Kernel;
    using Fitness.Domain;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public record EmailVerificationCommand(string Email, string EmailVerificationCode) : IRequest;

    internal sealed class EmailVerificationCommandValidator : AbstractValidator<EmailVerificationCommand>
    {
        public EmailVerificationCommandValidator()
        {
            this.RuleFor(x => x.EmailVerificationCode).NotEmpty();
        }
    }

    internal sealed class EmailVerificationCommandHandler : IRequestHandler<EmailVerificationCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public EmailVerificationCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
        {
            User user = await this.unitOfWork.Users.GetByEmailSafeAsync(request.Email, cancellationToken);

            // TODO: Transfer this to specification?
            if (
                !user.EmailVerification.IsVerified
                && user.EmailVerification.Code == request.EmailVerificationCode
                && user.EmailVerification.ExpirationDate > SystemClock.UtcNow)
            {
                user.VerifyEmail();

                this.unitOfWork.Users.Update(user);

                await this.unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
