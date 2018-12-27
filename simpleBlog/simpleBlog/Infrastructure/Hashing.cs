using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace simpleBlog.Infrastructure
{
    public class Hashing
    {
        public string getHashedPw(string password)
        {
            MD5 md5Hash = MD5.Create();
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(password.Trim());
            bs = csp.ComputeHash(bs);
            StringBuilder str = new StringBuilder();
            foreach (byte b in bs)
            {
                str.Append(b.ToString("x2").ToLower());
            }
            return str.ToString();
        }
    }
}