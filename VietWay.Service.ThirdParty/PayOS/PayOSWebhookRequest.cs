using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;

namespace VietWay.Service.ThirdParty.PayOS
{
    public record PayOSWebhookRequest(string code, string desc, bool isSuccess, WebhookData data, string signature) : WebhookType(code, desc, isSuccess, data, signature)
    {
    }
}
