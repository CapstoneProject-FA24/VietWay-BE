using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Middleware
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }
}
