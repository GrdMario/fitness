namespace Fitness.Infrastructure.Db.Users.Configurations
{
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(key => key.Id);

            builder.Property(p => p.Email).IsRequired();

            builder.Property(p => p.Password).IsRequired();

            builder.OwnsOne(p => p.EmailVerification, builder =>
            {
                builder.Property(p => p.IsVerified).IsRequired();
                builder.Property(p => p.ExpirationDate).IsRequired();
                builder.Property(p => p.Code).IsRequired();
                builder.ToTable("EmailVerifications");
            });

            builder.OwnsOne(x => x.Profile, builder =>
            {
                builder.ToTable("Profiles");

                builder.Property(p => p.FirstName);

                builder.Property(p => p.LastName);

                builder.Property(p => p.Mobile);
            });

            builder.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
