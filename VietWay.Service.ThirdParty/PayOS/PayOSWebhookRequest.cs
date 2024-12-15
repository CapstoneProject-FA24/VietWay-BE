using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;

namespace VietWay.Service.ThirdParty.PayOS
{
    public record PayOSWebhookRequest(string code, string desc, bool success, WebhookData data, string signature) : WebhookType(code, desc, success, data, signature)
    {
    }
}
