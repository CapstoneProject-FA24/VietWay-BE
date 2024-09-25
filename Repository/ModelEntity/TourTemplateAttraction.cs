using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    [PrimaryKey(nameof(TourTemplateId), nameof(AttractionId))]
    public class TourTemplateAttraction
    {
        [ForeignKey(nameof(TourTemplate))]
        public int TourTemplateId { get; set; }
        [ForeignKey(nameof(Attraction))]
        public int AttractionId { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Attraction? Attraction { get; set; }
    }
}
