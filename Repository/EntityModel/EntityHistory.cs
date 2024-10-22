using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class EntityHistory
    {
        [Key]
        public required string Id { get; set; }
        public required EntityType EntityType { get; set; }
        [StringLength(20)]
        public required string EntityId { get; set; }
        [StringLength(20)]
        public required string ModifiedBy { get; set; }
        public UserRole ModifierRole { get; set; }
        public string? Reason { get; set; }
        public required EntityModifyAction Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
