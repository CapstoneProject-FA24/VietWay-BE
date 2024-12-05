using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId), nameof(ProvinceId))]
    public class TourTemplateProvince
    {
        [ForeignKey(nameof(TourTemplate))]
        [Required]
        public string? TourTemplateId { get; set; }
        [ForeignKey(nameof(Province))]
        [Required]
        public string? ProvinceId { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Province? Province { get; set; }
    }
}
