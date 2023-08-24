namespace Fitness.Infrastructure.Adapter.Security.Internal
{
    using Fitness.Application.Contracts.Security;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    internal sealed class PasswordService : IPasswordService
    {
        private readonly PasswordSettings passwordSettings;

        private const string UpperCase = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
        private const string LowerCase = "abcdefghijkmnopqrstuvwxyz";
        private const string Digit = "0123456789";
        private const string Unique = "!@$?_-";
        private const string All = $"{UpperCase}{LowerCase}{Digit}{Unique}";


        public PasswordService(IOptions<SecurityAdapterSettings> settings)
        {
            this.passwordSettings = settings.Value.PasswordSettings;
        }

        public List<string> Validate(string password)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(password) || password.Length < this.passwordSettings.RequiredLength)
            {
                errors.Add("Password must be at least 8 digits.");
            }

            if (this.passwordSettings.RequireNonLetterOrDigit && password.All(IsLetterOrDigit))
            {
                errors.Add("Password requires non letter or digit.");
            }

            if (this.passwordSettings.RequireDigit && password.All(c => !IsDigit(c)))
            {
                errors.Add("Password must contain digits.");
            }

            if (this.passwordSettings.RequireLowercase && password.All(c => !IsLower(c)))
            {
                errors.Add("Password must containt lower case letters.");
            }

            if (this.passwordSettings.RequireUppercase && password.All(c => !IsUpper(c)))
            {
                errors.Add("Password must containt upper case letters.");
            }

            return errors;
        }

        public string Generate()
        {
            List<char> chars = new();

            if (this.passwordSettings.RequireUppercase)
            {
                chars.Add(UpperCase[RandomNumberGenerator.GetInt32(UpperCase.Length)]);
            }

            if (this.passwordSettings.RequireLowercase)
            {
                chars.Add(LowerCase[RandomNumberGenerator.GetInt32(LowerCase.Length)]);
            }

            if (this.passwordSettings.RequireDigit)
            {
                chars.Add(Digit[RandomNumberGenerator.GetInt32(Digit.Length)]);
            }

            if (this.passwordSettings.RequireNonLetterOrDigit)
            {
                chars.Add(Unique[RandomNumberGenerator.GetInt32(Unique.Length)]);
            }

            for (int i = chars.Count; i < this.passwordSettings.RequiredLength; i++)
            {
                chars.Add(All[RandomNumberGenerator.GetInt32(All.Length)]);
            }

            return new string(chars.ToArray());
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }


        private bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        private bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}
