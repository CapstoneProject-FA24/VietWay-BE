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
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public required DateTime CreatedDate { get; set; }

        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
