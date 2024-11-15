using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId),nameof(DayNumber),nameof(AttractionId))]
    public class AttractionSchedule
    {
        [StringLength(20)]
        [Required]
        public string? TourTemplateId { get; set; }
        [Required]
        public int DayNumber { get; set; }
        [ForeignKey(nameof(Attraction))]
        [StringLength(20)]
        [Required]
        public string? AttractionId { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual TourTemplateSchedule? TourTemplateSchedule { get; set; }
    }
}
