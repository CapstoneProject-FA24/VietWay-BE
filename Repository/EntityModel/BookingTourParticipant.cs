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
    public class BookingTourParticipant
    {
        [Key]
        [StringLength(20)]
        public required string ParticipantId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(TourBooking))]
        public required string TourBookingId { get; set; }
        public required string FullName { get; set; }
        [StringLength(10)]
        public required string PhoneNumber { get; set; }
        public required Gender Gender { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public bool? HasAttended { get; set; }
        public string? Note { get; set; }

        public virtual TourBooking? TourBooking { get; set; }
    }
}
