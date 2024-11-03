using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId), nameof(DayNumber))]
    public class TourTemplateSchedule
    {
        [ForeignKey(nameof(TourTemplate))]
        public required string TourTemplateId { get; set; }
        public int DayNumber { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<AttractionSchedule>? AttractionSchedules { get; set; }
        public virtual ICollection<EventSchedule>? EventSchedules { get; set; }
    }
}
