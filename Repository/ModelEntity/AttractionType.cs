using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class AttractionType
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttractionTypeId { get; set; }
        [Required]
        public required string AttractionTypeName { get; set; }
        [Required]
        public required string AttractionTypeDescription { get; set; }
        public required DateTime CreateDate { get; set; }

        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
