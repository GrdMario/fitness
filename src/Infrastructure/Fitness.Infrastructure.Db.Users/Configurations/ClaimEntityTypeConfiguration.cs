namespace Fitness.Infrastructure.Db.Users.Configurations
{
    using Fitness.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class Test { public Guid Id { get; set; } }

    internal sealed class TestEn : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }

    internal sealed class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToTable("Claims");

            builder.HasKey(key => key.Id);

            builder.Property(p => p.UserId).IsRequired();

            builder.Property(p => p.Value).IsRequired();
        }
    }
}
