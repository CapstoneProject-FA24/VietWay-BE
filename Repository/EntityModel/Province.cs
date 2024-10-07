using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    public class Province
    {
        [Key]
        [StringLength(20)]
        public required string ProvinceId { get; set; }
        [StringLength(50)]
        public required string ProvinceName { get; set; }
        [ForeignKey(nameof(Image))]
        [StringLength(20)]
        public required string ImageId { get; set; }

        public virtual Image? Image { get; set; }
    }
}
