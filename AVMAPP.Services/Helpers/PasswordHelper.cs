using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Services.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            var hashOfInput = HashPassword(enteredPassword);
            return hashOfInput == storedHashedPassword;
        }
    }
}
