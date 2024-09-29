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
    public class AttractionSchedule
    {
        public long TourTemplateId { get; set; }
        public int DayNumber { get; set; }
        [ForeignKey(nameof(Attraction))]
        [Required]
        public long AttractionId { get; set; }

        public virtual Attraction? Attraction { get; set; }
    }
}
