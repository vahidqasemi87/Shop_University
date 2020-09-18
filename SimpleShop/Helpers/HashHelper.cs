using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleShop.Helpers
{
    public class HashHelper
    {
        public string GetMD5(string input)
        {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
