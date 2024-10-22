using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace VietWay.Repository.EntityModel
{
    public class TourTemplateImage
    {
        [Key]
        [ForeignKey(nameof(Image))]
        public required string ImageId { get; set; }
        [ForeignKey(nameof(TourTemplate))]
        public required string TourTemplateId { get; set; }
        [Required]
        [StringLength(2048)]
        public required string ImageUrl { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
    }
}
