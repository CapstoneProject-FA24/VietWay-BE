using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class TourCategory
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TourCategoryId { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
