namespace Fitness.Application.Contracts.Email
{
    using Fitness.Domain;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmailVerificationAsync(User user);

        Task SendPasswordResetEmailAsync(User user);
    }
}
