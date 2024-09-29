using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class AttractionType
    {
        [Key]
        public long AttractionTypeId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(Creator))]
        public long CreatedBy { get; set; }

        public virtual ManagerInfo? Creator { get; set; }
        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
