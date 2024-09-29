using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Attraction : CreatedByEntity<Staff>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AttractionId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Address { get; set; }
        [Required]
        public required string ContactInfo { get; set; }
        public string? Website { get; set; }
        [Required]
        public required string Description { get; set; }
        [ForeignKey(nameof(AttractionType))]
        public long AttractionTypeId { get; set; }
        public string? GoogleMapUrl { get; set; }

        public virtual AttractionType? AttractionType { get; set; }
        public virtual ICollection<AttractionImage>? AttractionImages { get; set; }
    }
}
