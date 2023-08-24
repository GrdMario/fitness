namespace Fitness.Infrastructure.Db.Users.Configurations
{
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(key => key.Id);

            builder.Property(p => p.UserId).IsRequired();

            builder.Property(p => p.CreatedAt).IsRequired();

            builder.Property(p => p.ExpiresAt).IsRequired();
        }
    }
}
