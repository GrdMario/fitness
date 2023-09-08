namespace Fitness.Infrastructure.Db.Users.Repositories
{
    using Fitness.Application.Contracts;
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class FileRepository : IFileRepository
    {
        private readonly DbSet<File> files;

        public FileRepository(UsersDbContext context)
        {
            this.files = context.Set<File>();
        }

        public void Add(File file)
        {
            this.files.Add(file);
        }

        public void Delete(File file)
        {
            this.files.Remove(file);
        }

        public async Task<File?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.files.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<File> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.GetByIdAsync(id, cancellationToken) ?? throw new ApplicationException("Not found");
        }
    }
}
