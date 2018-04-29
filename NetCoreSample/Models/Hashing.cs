﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography;

namespace NetCoreSample.Models
{
    public static class Hashing
    {
        private static string GetRandomSalt()
        {

            //return Crypto.GenerateSalt(10);
            return "";
        }

        /// <summary>
        /// Password To hash
        /// </summary>
        /// <param name="password"></param>
        /// <returns>hash 過的密碼</returns>
        public static string HashPassword(this string password)
        {
            return "";
            //return Crypto.HashPassword(password);
        }

        /// <summary>
        /// Check Hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword">Hash過的密碼(DB來ㄉ)</param>
        /// <returns></returns>
        public static bool ValidatePassword(this string password, string hashedPassword)
        {
            return true;
            //return Crypto.VerifyHashedPassword(hashedPassword, password);
        }

        public const string Alphabet ="abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateString(int size)
        {
            Random rand = new Random();
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }
    }
}
