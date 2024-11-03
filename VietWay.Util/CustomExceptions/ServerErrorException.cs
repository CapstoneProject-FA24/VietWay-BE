using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.CustomExceptions
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException() { }
        public ServerErrorException(string message) : base(message) { }
        public ServerErrorException(string message, Exception inner) : base(message, inner) { }
    }
}
