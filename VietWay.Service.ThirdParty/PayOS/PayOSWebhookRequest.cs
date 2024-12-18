using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;

namespace VietWay.Service.ThirdParty.PayOS
{
    public class PayOSWebhookRequest
    {
        public required string Code { get; set; }
        public required string Desc { get; set; }
        public bool Success { get; set; }
        public required PayOSWebhookData Data { get; set; }
        public required string Signature { get; set; }
    }
}
