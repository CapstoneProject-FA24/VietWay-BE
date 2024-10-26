using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId), nameof(DayNumber),nameof(EventId))]
    public class EventSchedule
    {
        [StringLength(20)]
        public required string TourTemplateId { get; set; }
        public required int DayNumber { get; set; }
        [ForeignKey(nameof(Event))]
        [StringLength(20)]
        public required string EventId { get; set; }

        public virtual Event? Event { get; set; }
        public virtual TourTemplateSchedule? TourTemplateSchedule { get; set; }
    }
}
