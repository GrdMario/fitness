namespace Fitness.Domain
{
    using System;

    public class Exercise
    {
        private Exercise() { }

        public Guid Id { get; }

        public string Name { get; } = default!;

        public string Description { get; } = default!;

        public DateTimeOffset CreatedAt { get; }

        public bool IsDeleted { get; }
    }
}
