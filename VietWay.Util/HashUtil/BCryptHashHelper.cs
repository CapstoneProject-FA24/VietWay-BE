using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace VietWay.Util.HashUtil
{
    public class BCryptHashHelper : IHashHelper
    {
        public string Hash(string input)
        {
            return BC.EnhancedHashPassword(input, 12);
        }

        public bool Verify(string input, string hash)
        {
            return BC.EnhancedVerify(input, hash);
        }
    }
}
