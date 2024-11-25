using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class EntityStatusHistory
    {
        [Key]
        [ForeignKey(nameof(EntityHistory))]
        [Required]
        public string? Id { get; set; }
        [Required]
        public int OldStatus { get; set; }
        [Required]
        public int NewStatus { get; set; }

        public virtual EntityHistory? EntityHistory { get; set; }
    }
}
