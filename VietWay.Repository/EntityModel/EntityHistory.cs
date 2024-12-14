using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class EntityHistory
    {
        [Key]
        [Required]
        public string? Id { get; set; }
        [Required]
        public EntityType EntityType { get; set; }
        [StringLength(20)]
        [Required]
        public string? EntityId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Modifier))] 
        [Required]
        public string? ModifiedBy { get; set; }
        [Required]
        public UserRole ModifierRole { get; set; }
        public string? Reason { get; set; }
        [Required]
        public EntityModifyAction Action { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

        public virtual EntityStatusHistory? StatusHistory { get; set; }
        public virtual Account? Modifier { get; set; }
    }
}
