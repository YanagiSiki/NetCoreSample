using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography;
using System.Web.Helpers;

namespace NetCoreSample.Models
{
    public static class Hashing
    {
        private static string GetRandomSalt()
        {
            
            return Crypto.GenerateSalt(10);
        }

        /// <summary>
        /// Password To hash
        /// </summary>
        /// <param name="password"></param>
        /// <returns>hash 過的密碼</returns>
        public static string HashPassword(this string password)
        {
            return Crypto.HashPassword(password);
        }

        /// <summary>
        /// Check Hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword">Hash過的密碼(DB來ㄉ)</param>
        /// <returns></returns>
        public static bool ValidatePassword(this string password, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, password);
        }
    }
}
