using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class BookingPayment
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [ForeignKey(nameof(TourBooking))]
        public int BookingId { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public required string PaymentMethod { get; set; }
        public Enum.PaymentStatus PaymentStatus { get; set; }
        public virtual TourBooking? TourBooking { get; set; }
        public virtual ICollection<Transaction>? Transaction { get; set; }
    }
}
