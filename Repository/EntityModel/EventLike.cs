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
    [PrimaryKey(nameof(EventId),nameof(CustomerId))]
    public class EventLike
    {
        [StringLength(20)]
        [ForeignKey(nameof(Event))]
        public required string EventId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }

        public virtual Event? Event { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
