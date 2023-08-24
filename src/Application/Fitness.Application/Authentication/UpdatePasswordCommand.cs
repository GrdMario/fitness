namespace Fitness.Application.Authentication
{
    using Fitness.Application.Authentication.Validators;
    using Fitness.Application.Contracts;
    using Fitness.Application.Contracts.Email;
    using Fitness.Application.Contracts.Security;
    using Fitness.Blocks.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public record UpdatePasswordCommand(string Email, string OldPassword, string NewPassword) : IRequest;

    internal sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator(IPasswordService passwordService)
        {
            this.RuleFor(r => r.Email).EmailAddress();

            this.RuleFor(r => r.NewPassword)
                .SetValidator(new PasswordValidator(passwordService));
        }
    }

    internal sealed class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHashService passwordHashService;
        private readonly IEmailService emailService;

        public UpdatePasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordHashService passwordHashService, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHashService = passwordHashService;
            this.emailService = emailService;
        }

        public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await this.unitOfWork.Users.GetByEmailSafeAsync(request.Email, cancellationToken);

            var isValidOldPassword = this.passwordHashService.VerifyHash(user.Password, request.OldPassword);

            if (!isValidOldPassword)
            {
                throw new ServiceValidationException("Invalid credentials.");
            }

           var newPasswordHash = this.passwordHashService.Hash(request.NewPassword);

            user.SetPassword(newPasswordHash);

            this.unitOfWork.Users.Update(user);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            await this.emailService.SendPasswordResetEmailAsync(user);
        }
    }
}
