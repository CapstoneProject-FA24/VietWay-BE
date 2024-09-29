using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class Attraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AttractionId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ContactInfo { get; set; }
        public string? Website { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(Creator))]
        public long CreatedBy { get; set; }
        [ForeignKey(nameof(AttractionType))]
        public long AttractionTypeId { get; set; }
        public string? GoogleMapUrl { get; set; }

        public virtual ManagerInfo? Creator { get; set; }
        public virtual AttractionType? AttractionType { get; set; }
        public virtual ICollection<AttractionImage>? AttractionImages { get; set; }
    }
}
