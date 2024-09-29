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
    public class CustomerFeedback : SoftDeleteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long FeedbackId { get; set; }
        [ForeignKey(nameof(Booking))]
        public long BookingId { get; set; }
        public int Rating { get; set; }
        [Required]
        public required string Feedback { get; set; }

        public virtual TourBooking? Booking { get; set; }
    }
}
