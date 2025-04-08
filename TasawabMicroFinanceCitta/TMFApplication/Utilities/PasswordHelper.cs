using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Security.Cryptography;

namespace TMFApplication.Utilities
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Use a secure hashing algorithm like BCrypt or Argon2
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Use the same hashing algorithm to verify the password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public static string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }
    }
}