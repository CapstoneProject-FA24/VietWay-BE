using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class EventCategory : SoftDeleteEntity
    {
        [Key]
        public required string EventCategoryId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual ICollection<Event>? Events { get; set; }
    }
}
