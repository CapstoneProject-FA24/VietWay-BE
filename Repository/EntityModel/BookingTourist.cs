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
    public class BookingTourist
    {
        [Key]
        [StringLength(20)]
        public required string TouristId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Booking))]
        public required string BookingId { get; set; }
        public required string FullName { get; set; }
        [StringLength(10)]
        public required string PhoneNumber { get; set; }
        public required Gender Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public bool? HasAttended { get; set; }
        [Range(0.01, 999999999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        public string? Note { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
