using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class TourRefundPolicy
    {
        [Key]
        [StringLength(20)]
        public required string TourRefundPolicyId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(TourId))]
        public required string TourId { get; set; }
        public required DateTime CancelBefore { get; set; }
        public required decimal RefundPercent { get; set; }

        public virtual Tour? Tour { get; set; }

    }
}
