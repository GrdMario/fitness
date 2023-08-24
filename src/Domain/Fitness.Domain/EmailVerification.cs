namespace Fitness.Domain
{
    using System;

    public class EmailVerification
    {
        public EmailVerification(
            bool isVerified,
            string code,
            DateTimeOffset expirationDate)
        {
            this.IsVerified = isVerified;
            this.Code = code;
            this.ExpirationDate = expirationDate;
        }

        public bool IsVerified { get; protected set; }

        public string Code { get; protected set; }

        public DateTimeOffset ExpirationDate { get; protected set; }

        public void Verify()
        {
            this.IsVerified = true;
        }
    }
}
