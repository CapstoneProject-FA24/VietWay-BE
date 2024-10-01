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
    [PrimaryKey(nameof(TourTemplateId),nameof(DayNumber),nameof(AttractionId))]
    public class AttractionSchedule
    {
        [StringLength(20)]
        public required string TourTemplateId { get; set; }
        public required int DayNumber { get; set; }
        [ForeignKey(nameof(Attraction))]
        [StringLength(20)]
        public required string AttractionId { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual TourTemplateSchedule? TourTemplateSchedule { get; set; }
    }
}
