using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

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
