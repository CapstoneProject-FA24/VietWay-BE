using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    [PrimaryKey(nameof(TourTemplateId), nameof(ImageId))]
    public class TourTemplateImage
    {
        [ForeignKey(nameof(TourTemplate))]
        public long TourTemplateId { get; set; }
        [ForeignKey(nameof(Image))]
        public long ImageId { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Image? Image { get; set; }
    }
}
