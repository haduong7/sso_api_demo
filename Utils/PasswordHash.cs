using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities.Security
{
    public class PasswordHash
    {
        public static string MD5Encrypt(string plainText)
        {
            UTF8Encoding encoding1 = new UTF8Encoding();
            MD5CryptoServiceProvider provider1 = new MD5CryptoServiceProvider();
            byte[] buffer1 = encoding1.GetBytes(plainText);
            byte[] buffer2 = provider1.ComputeHash(buffer1);
            return BitConverter.ToString(buffer2).Replace("-", "").ToLower();
        }

        public static string EncryptPassword(string Md5password, string salt)
        {
            return MD5Encrypt(string.Format("{0}{1}", Md5password, salt));
        }

        public static string RandomString(int length)
        {
            string text1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int num1 = text1.Length;
            Random random1 = new Random();
            string text2 = string.Empty;
            for (int num2 = 0; num2 < length; num2++)
            {
                text2 = string.Format("{0}{1}", text2, text1[random1.Next(num1)]);
            }
            return text2;
        }
    }
}
