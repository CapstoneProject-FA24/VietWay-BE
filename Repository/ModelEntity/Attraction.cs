using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class Attraction
    {
        public int AttractionId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ContactInfo { get; set; }
        public string? Website { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(ManagerInfo))]
        public int CreatedBy { get; set; }
        [ForeignKey(nameof(AttractionType))]
        public int AttractionTypeId { get; set; }
        public Enum.AttractionStatus AttractionStatus { get; set; }
        public string? GoogleMapUrl { get; set; }

        public virtual ManagerInfo? ManagerInfo { get; set; }
        public virtual AttractionType? AttractionType { get; set; }
        public virtual ICollection<AttractionImage>? AttractionImages { get; set; }
    }
}
