using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class Tour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TourId { get; set; }
        [ForeignKey(nameof(TourTemplate))]
        public long TourTemplateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int MaxParticipant { get; set; }
        public int MinParticipant { get; set; }
        public int CurrentParticipant { get; set; }
        public TourStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(StaffInfo))]
        public long CreatedBy { get; set; }

        public virtual StaffInfo? Creator { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<TourBooking>? Bookings { get; set; }

    }
}
