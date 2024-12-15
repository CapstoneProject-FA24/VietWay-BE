using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.CustomExceptions
{
    public class InvalidActionException : Exception
    {
        public InvalidActionException(string message) : base(message)
        {
        }
        public InvalidActionException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public InvalidActionException() :base()
        {
        }
    }
}
