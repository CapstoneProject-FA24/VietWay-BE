using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class TourPrice
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? PriceId { get; set; }
        [ForeignKey(nameof(Tour))]
        [StringLength(20)]
        [Required]
        public string? TourId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
        [Required]  
        public int AgeFrom { get; set; }
        [Required]
        public int AgeTo { get; set; }

        public virtual Tour? Tour { get; set; }
    }
}
