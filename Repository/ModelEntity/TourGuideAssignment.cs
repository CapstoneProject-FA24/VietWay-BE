using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.ModelEntity
{
    [PrimaryKey(nameof(TourId), nameof(TourGuideId))]
    public class TourGuideAssignment
    {
        [ForeignKey(nameof(Tour))]
        public int TourId { get; set; }
        [ForeignKey(nameof(TourGuideInfo))]
        public int TourGuideId { get; set; }
        public DateTime AssignedDate { get; set; }
        [ForeignKey(nameof(CompanyStaffInfo))]
        public int AssignedBy { get; set; }

        public virtual CompanyStaffInfo? CompanyStaffInfo { get; set; }
        public virtual Tour? Tour { get; set; }
        public virtual TourGuideInfo? TourGuideInfo { get; set; }
    }
}
