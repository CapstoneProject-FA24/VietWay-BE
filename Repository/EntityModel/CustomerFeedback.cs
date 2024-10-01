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
        [StringLength(20)]
        public required string FeedbackId { get; set; }
        [ForeignKey(nameof(Booking))]
        [StringLength(20)]
        public required string BookingId { get; set; }
        public required int Rating { get; set; }
        public required string Feedback { get; set; }

        public virtual TourBooking? Booking { get; set; }
    }
}
