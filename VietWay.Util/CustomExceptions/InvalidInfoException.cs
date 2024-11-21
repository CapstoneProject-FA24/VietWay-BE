using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.CustomExceptions
{
    public class InvalidInfoException : Exception
    {
        public InvalidInfoException(string message) : base(message)
        {
        }
        public InvalidInfoException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public InvalidInfoException() : base()
        {
        }
    }
}
