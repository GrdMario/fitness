namespace Fitness.Infrastructure.Db.Users.Configurations
{
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class FileEntityTypeConfiguration : IEntityTypeConfiguration<File>
    {
        private const string TableName = "Files";

        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(e => e.Id);

            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.IsDeleted).IsRequired();

            builder.Property(p => p.CreatedAt).IsRequired();

            builder.Property(p => p.FileLength).IsRequired();

            builder.Property(p => p.EntityId).IsRequired();

            builder.Property(p => p.FileExtensionId).IsRequired();

            builder.Property(p => p.FileTypeId).IsRequired();
        }
    }
}
