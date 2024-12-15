using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.PayOS
{
    public class PayOSConfiguration
    {
        public required string ApiKey { get; set; }
        public required string ChecksumKey { get; set; }
        public required string CLientId { get; set; }
        public required string ReturnUrl { get; set; }
    }
}
