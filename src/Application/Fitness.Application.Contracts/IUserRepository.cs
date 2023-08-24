namespace Fitness.Application.Contracts
{
    using Fitness.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        void Create(User user);
        void Update(User user);
        void Delete(User user);
        Task<User> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task<User> GetByEmailSafeAsync(string email, CancellationToken cancellationToken);
    }
}
