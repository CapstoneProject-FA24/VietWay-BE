using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId), nameof(ProvinceId))]
    public class TourTemplateProvince
    {
        [ForeignKey(nameof(TourTemplate))]
        public required string TourTemplateId { get; set; }
        [ForeignKey(nameof(Province))]
        public required string ProvinceId { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Province? Province { get; set; }
    }
}
