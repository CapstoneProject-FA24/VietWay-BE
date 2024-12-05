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
        [Required]
        public string? ImageId { get; set; }
        [ForeignKey(nameof(TourTemplate))]
        [Required]
        public string? TourTemplateId { get; set; }
        [Required]
        [StringLength(2048)]
        public string? ImageUrl { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
    }
}
