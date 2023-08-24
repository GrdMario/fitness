namespace Fitness.Infrastructure.Db.Users.Configurations
{
    using Fitness.Domain;
    using Fitness.Domain.Seedwork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class FileExtensionEntityTypeConfiguration : IEntityTypeConfiguration<FileExtension>
    {
        private const string TableName = "FileExtensions";

        public void Configure(EntityTypeBuilder<FileExtension> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(e => e.Id);

            builder.Property(p => p.Name).IsRequired();

            builder.HasData(Enumeration.GetAll<FileExtension>());
        }
    }
}
