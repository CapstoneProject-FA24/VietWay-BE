using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class Tour
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TourId { get; set; }
        [ForeignKey(nameof(TourTemplate))]
        public int TourTemplateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int MaxParticipant { get; set; }
        public int MinParticipant { get; set; }
        public int CurrentParticipant { get; set; }
        public Enum.TourStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(CompanyStaffInfo))]
        public int CreatedBy { get; set; }

        public virtual CompanyStaffInfo? CompanyStaffInfo { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<TourBooking>? Bookings { get; set; }
        public virtual ICollection<TourGuideAssignment>? TourGuideAssignments { get; set; }

    }
}
