namespace Fitness.Domain
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        private User() { }

        public User(
            Guid id,
            string email,
            string password,
            Profile profile,
            EmailVerification emailVerification,
            List<Claim> claims)
        {
            this.Id = id;

            this.Email = email;

            this.Password = password;

            this.Profile = profile;

            this.EmailVerification = emailVerification;

            this.Claims = claims;
        }

        public Guid Id { get; protected set; }

        public string Email { get; protected set; } = default!;

        public string Password { get; protected set; } = default!;

        public EmailVerification EmailVerification { get; protected set; } = default!;

        public Profile Profile { get; protected set; } = default!;

        public List<Claim> Claims { get; protected set; } = new();

        public void AddEmailVerification(EmailVerification emailVerification)
        {
            this.EmailVerification = emailVerification;
        }

        public void VerifyEmail()
        {
            this.EmailVerification.Verify();
        }

        public void SetPassword(string password)
        {
            this.Password = password;
        }
    }
}
