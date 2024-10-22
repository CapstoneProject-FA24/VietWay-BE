using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    public class AttractionImage
    {
        [Key]
        [StringLength(20)]
        public required string ImageId { get; set; }
        [ForeignKey(nameof(Attraction))]
        [StringLength(20)]
        public required string AttractionId { get; set; }
        [StringLength(2048)]
        public required string ImageUrl { get; set; }

        public virtual Attraction? Attraction { get; set; }
    }
}
