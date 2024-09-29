using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class Province
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProvinceId { get; set; }
        public required string ProvinceName { get; set; }
        [ForeignKey(nameof(Image))]
        public required long ImageId { get; set; }

        public virtual Image? Image { get; set; }
    }
}
