using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    [PrimaryKey("TourTemplateId", "LocationId")]
    public class TourTemplateLocation
    {
        [ForeignKey(nameof(TourTemplate))]
        public int TourTemplateId { get; set; }
        [ForeignKey(nameof(Province))]
        public int ProvinceId { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual Province? Province { get; set; }
    }
}
