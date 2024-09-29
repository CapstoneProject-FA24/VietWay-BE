using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class TourTemplateSchedule
    {
        [ForeignKey(nameof(TourTemplate))]
        public long TourTemplateId { get; set; }
        public int DayNumber { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<AttractionSchedule>? AttractionSchedules { get; set; }
    }
}
