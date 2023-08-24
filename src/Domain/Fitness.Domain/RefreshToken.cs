namespace Fitness.Domain
{
    using System;

    public class RefreshToken
    {
        public RefreshToken(
            Guid id,
            Guid userId,
            DateTimeOffset expiresAt,
            DateTimeOffset createdAt)
        {
            this.Id = id;
            this.UserId = userId;
            this.ExpiresAt = expiresAt;
            this.CreatedAt = createdAt;
        }

        public Guid Id { get; protected set; }

        public Guid UserId { get; protected set; }

        public DateTimeOffset ExpiresAt { get; protected set; }

        public DateTimeOffset CreatedAt { get; protected set; }
    }
}
