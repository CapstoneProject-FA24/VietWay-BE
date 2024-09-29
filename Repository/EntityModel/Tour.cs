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
    public class Tour : CreatedByEntity<Staff>
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

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<TourBooking>? Bookings { get; set; }

    }
}
