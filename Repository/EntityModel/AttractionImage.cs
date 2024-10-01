using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(20)]
        public required string AttractionId { get; set; }
        [ForeignKey(nameof(Image))]
        [StringLength(20)]
        public required string ImageId { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual Image? Image { get; set; }
    }
}
