namespace Fitness.Infrastructure.Adapter.Email.Internal
{
    using Fitness.Application.Contracts.Email;
    using Fitness.Domain;
    using System.Threading.Tasks;

    internal sealed class EmailService : IEmailService
    {
        public async Task SendEmailVerificationAsync(User user)
        {
            await Task.FromResult(0);
        }

        public async Task SendPasswordResetEmailAsync(User user)
        {
            await Task.FromResult(0);
        }
    }
}
