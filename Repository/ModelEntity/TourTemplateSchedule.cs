using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    [PrimaryKey("TourTemplateId", "DayNumber")]
    public class TourTemplateSchedule
    {
        [ForeignKey(nameof(TourTemplate))]
        public int TourTemplateId { get; set; }
        public int DayNumber { get; set; }
        public string ScheduleTitle { get; set; }
        public string ScheduleDescription { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
    }
}
