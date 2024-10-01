using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class BookingPayment
    {
        [Key]
        [StringLength(20)]
        public required string PaymentId { get; set; }
        [ForeignKey(nameof(TourBooking))]
        [StringLength(20)]
        public required string BookingId { get; set; }
        public required DateTime PaymentDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Amount { get; set; }
        [StringLength(50)]
        public required string PaymentMethod { get; set; }
        public required PaymentStatus PaymentStatus { get; set; }

        public virtual TourBooking? TourBooking { get; set; }
        public virtual ICollection<Transaction>? Transaction { get; set; }
    }
}
