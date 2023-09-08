namespace Fitness.Application.Contracts
{
    using Fitness.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFileRepository
    {
        Task<File?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<File> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken);

        void Add(File file);

        void Delete(File file);
    }
}
