using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourRefundPolicyDTO
    {
        public required DateTime CancelBefore { get; set; }
        public required decimal RefundPercent { get; set; }
    }
}
