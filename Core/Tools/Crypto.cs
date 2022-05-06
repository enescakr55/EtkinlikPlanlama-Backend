using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Tools
{
    public static class Crypto
    {
        public static string MD5Crypt(string input)
        {
            using(MD5 md5 = MD5.Create())
            {
                byte[] inputbytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputbytes);
                string str = BitConverter.ToString(hash).Replace("-", "");
                return str;
            }
        }
    }
}
