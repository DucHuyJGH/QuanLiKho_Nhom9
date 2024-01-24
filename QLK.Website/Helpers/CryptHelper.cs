using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QLK.Website.Helpers
{
    public static class CryptHelper
    {
        public static string Md5(String text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encodeer = new UTF8Encoding();
            Byte[] originalBytes = encodeer.GetBytes(text);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            text = BitConverter.ToString(encodedBytes).Replace("-", "");
            var result = text.ToLower();
            return result;
        }
    }
}