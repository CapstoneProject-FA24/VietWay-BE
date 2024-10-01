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
        [StringLength(20)]
        public required string AttractionTypeId { get; set; }
        [StringLength(100)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
