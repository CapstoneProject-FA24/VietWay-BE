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
        [Required]
        public string? TouristId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Booking))]
        [Required]
        public string? BookingId { get; set; }
        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }
        [StringLength(10)]
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public bool? HasAttended { get; set; }
        [Range(0.01, 999999999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
        [StringLength(12)]
        public string? PIN { get; set; }
        public string? Note { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
