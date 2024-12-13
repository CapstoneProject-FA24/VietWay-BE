using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Province : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? ProvinceId { get; set; }
        [StringLength(50)]
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [StringLength(2048)]
        [Required]
        public string? ImageUrl { get; set; }

        public virtual ICollection<Attraction>? Attractions { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<TourTemplateProvince>? TourTemplateProvinces { get; set; }
    }
}
