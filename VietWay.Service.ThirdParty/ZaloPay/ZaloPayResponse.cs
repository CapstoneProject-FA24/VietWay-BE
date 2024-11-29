using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.ZaloPay
{
    public class ZaloPayResponse
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public int SubReturnCode { get; set; }
        public string SubReturnMessage { get; set; }
        public bool IsProcessing { get; set; }
        public long Amount { get; set; }
        public long DiscountAmount { get; set; }
        public long ZpTransId { get; set; }
        public long ServerTime { get; set; }
    }
}
