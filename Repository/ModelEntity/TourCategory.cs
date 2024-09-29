using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class TourCategory
    {
        [Key]
        public long TourCategoryId { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
