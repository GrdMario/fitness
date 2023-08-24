namespace Fitness.Domain
{
    using System;

    public class Profile
    {
        public Profile(
            Guid userId,
            string? firstName,
            string? lastName,
            string? mobile)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Mobile = mobile;
        }

        public Guid UserId { get; protected set; }

        public string? FirstName { get; protected set; }

        public string? LastName { get; protected set; }

        public string? Mobile { get; protected set; }
    }
}
