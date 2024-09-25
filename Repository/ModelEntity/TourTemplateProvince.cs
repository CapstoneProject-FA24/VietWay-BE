using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.ModelEntity
{
    [PrimaryKey(nameof(TourTemplateId), nameof(ProvinceId))]
    public class TourTemplateProvince
    {
        [ForeignKey(nameof(TourTemplate))]
        public int TourTemplateId { get; set; }
        [ForeignKey(nameof(Province))]
        public int ProvinceId { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Province? Province { get; set; }
    }
}
