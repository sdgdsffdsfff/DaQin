using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utils
{
    public class MD5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        public static string Encrypt(string text)
        {
            byte[] bArr = ASCIIEncoding.UTF8.GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            bArr = md5.ComputeHash(bArr);
            StringBuilder result = new StringBuilder();
            foreach (byte b in bArr)
            {
                result.Append(b.ToString("X2"));
            }
            return result.ToString();
        }
    }
}
