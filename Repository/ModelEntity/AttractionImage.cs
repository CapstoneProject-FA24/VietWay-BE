using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    [PrimaryKey("AttractionId", "ImageId")]
    public class AttractionImage
    {
        [ForeignKey(nameof(Attraction))]
        public int AttractionId { get; set; }
        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual Image? Image { get; set; }
    }
}
