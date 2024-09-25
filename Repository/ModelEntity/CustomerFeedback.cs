using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class CustomerFeedback
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }
        [ForeignKey(nameof(Booking))]
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public required string Feedback { get; set; }

        public virtual TourBooking? Booking { get; set; }
    }
}
