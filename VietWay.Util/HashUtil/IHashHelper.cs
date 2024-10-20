using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.HashUtil
{
    public interface IHashHelper
    {
        public string Hash(string input);
        public bool Verify(string input, string hash);
    }
}
