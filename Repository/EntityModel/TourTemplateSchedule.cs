using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId), nameof(DayNumber))]
    public class TourTemplateSchedule
    {
        [ForeignKey(nameof(TourTemplate))]
        [Required]
        public string? TourTemplateId { get; set; }
        [Required]
        public int DayNumber { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<AttractionSchedule>? AttractionSchedules { get; set; }
    }
}
