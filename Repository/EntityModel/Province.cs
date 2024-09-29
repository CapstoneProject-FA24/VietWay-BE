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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProvinceId { get; set; }
        [Required]
        public required string ProvinceName { get; set; }
        [ForeignKey(nameof(Image))]
        public long ImageId { get; set; }

        public virtual Image? Image { get; set; }
    }
}
