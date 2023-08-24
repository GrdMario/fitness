namespace Fitness.Infrastructure.Adapter.Security.Internal
{
    using Fitness.Application.Contracts.Security;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    internal sealed class PasswordHashService : IPasswordHashService
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
        private static readonly KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1;
        private static readonly int Pbkdf2IterCount = 1000;
        private static readonly int Pbkdf2SubkeyLength = 256 / 8;
        private static readonly int SaltSize = 128 / 8;

        public string Hash(string password)
        {
            var hash = HashPassword(password);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyHash(string password, string passwordGuess)
        {
            if (password is null || passwordGuess is null)
            {
                return false;
            }

            return VerifyPasswordHash(password, passwordGuess);
        }

        private static byte[] HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];

            Rng.GetBytes(salt);

            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];

            outputBytes[0] = 0x00;

            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);

            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);

            return outputBytes;
        }

        private static bool VerifyPasswordHash(string password, string guess)
        {
            byte[] hashedPassword = Convert.FromBase64String(password);

            if (hashedPassword == null || hashedPassword.Length == 0)
            {
                return false;
            }

            if (hashedPassword.Length != 1 + SaltSize + Pbkdf2SubkeyLength)
            {
                return false;
            }

            byte[] salt = new byte[SaltSize];

            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[Pbkdf2SubkeyLength];

            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(guess, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            return ByteArraysEqual(actualSubkey, expectedSubkey);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;

            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }

            return areSame;
        }
    }
}
