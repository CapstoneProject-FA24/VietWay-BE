using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    public class AttractionImage
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? ImageId { get; set; }
        [ForeignKey(nameof(Attraction))]
        [StringLength(20)]
        [Required]
        public string? AttractionId { get; set; }
        [StringLength(2048)]
        [Required]
        public string? ImageUrl { get; set; }

        public virtual Attraction? Attraction { get; set; }
    }
}
