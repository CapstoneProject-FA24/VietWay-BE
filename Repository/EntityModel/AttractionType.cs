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
    public class AttractionType : CreatedByEntity<Manager>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AttractionTypeId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
