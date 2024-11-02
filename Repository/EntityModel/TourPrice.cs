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
        public required string PriceId { get; set; }
        [ForeignKey(nameof(Tour))]
        [StringLength(20)]
        public required string TourId { get; set; }
        public required string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        public required int AgeFrom { get; set; }
        public required int AgeTo { get; set; }

        public virtual Tour? Tour { get; set; }
    }
}
