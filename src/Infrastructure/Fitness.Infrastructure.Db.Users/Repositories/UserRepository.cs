namespace Fitness.Infrastructure.Db.Users.Repositories
{
    using Fitness.Application.Contracts;
    using Fitness.Blocks.Common.Exceptions;
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class UserRepository : IUserRepository
    {
        private readonly DbSet<User> users;

        public UserRepository(UsersDbContext context)
        {
            this.users = context.Set<User>();
        }
        public void Create(User user)
        {
            this.users.Add(user);
        }

        public void Delete(User user)
        {
            this.users.Remove(user);
        }

        public async Task<User> GetByEmailSafeAsync(string email, CancellationToken cancellationToken)
        {
            return await this.GetByEmailAsync(email, cancellationToken)
                ?? throw new ServiceValidationException("User not found.");
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await this.users.Where(user => user.Email == email).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailVerificationCodeAsync(string emailVerificationCode, CancellationToken cancellationToken)
        {
            return await this.users
                .Where(user => user.EmailVerification.Code == emailVerificationCode)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.users.FindAsync(new object[] { id }, cancellationToken) ?? throw new ServiceValidationException("Unable to find that user.");
        }

        public void Update(User user)
        {
            this.users.Update(user);
        }
    }
}
