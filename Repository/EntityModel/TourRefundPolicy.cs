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
        [Required]
        public string? TourRefundPolicyId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(TourId))]
        [Required]
        public string? TourId { get; set; }
        [Required]
        public DateTime CancelBefore { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal RefundPercent { get; set; }

        public virtual Tour? Tour { get; set; }

    }
}
