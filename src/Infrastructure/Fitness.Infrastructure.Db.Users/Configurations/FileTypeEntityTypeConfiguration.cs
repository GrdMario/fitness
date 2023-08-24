namespace Fitness.Infrastructure.Db.Users.Configurations
{
    using Fitness.Domain;
    using Fitness.Domain.Seedwork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class FileTypeEntityTypeConfiguration : IEntityTypeConfiguration<FileType>
    {
        private const string TableName = "FileTypes";

        public void Configure(EntityTypeBuilder<FileType> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(e => e.Id);

            builder.Property(p => p.Name).IsRequired();

            builder.HasData(Enumeration.GetAll<FileType>());
        }
    }
}
