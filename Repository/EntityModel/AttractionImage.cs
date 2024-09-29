using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey("AttractionId", "ImageId")]
    public class AttractionImage
    {
        [ForeignKey(nameof(Attraction))]
        public long AttractionId { get; set; }
        [ForeignKey(nameof(Image))]
        public long ImageId { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual Image? Image { get; set; }
    }
}
