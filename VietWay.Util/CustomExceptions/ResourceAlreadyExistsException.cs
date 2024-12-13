using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.CustomExceptions
{
    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException() : base() { }
        public ResourceAlreadyExistsException(string message) : base(message) { }
        public ResourceAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
