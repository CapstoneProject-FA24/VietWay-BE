using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(TourTemplateId), nameof(ImageId))]
    public class TourTemplateImage
    {
        [ForeignKey(nameof(TourTemplate))]
        public string TourTemplateId { get; set; }
        [ForeignKey(nameof(Image))]
        public string ImageId { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Image? Image { get; set; }
    }
}
