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
        public required string AttractionName { get; set; }
        public required string AttractionAddress { get; set; }
        public required string AttractionPhone { get; set; }
        public required string AttractionEmail { get; set; }
        public string? AttractionWebsite { get; set; }
        public required string AttractionDescription { get; set; }
        public required DateTime CreateDate { get; set; }
        [ForeignKey(nameof(AttractionType))]
        public int AttractionTypeId { get; set; }
        public Enum.AttractionStatus AttractionStatus { get; set; }

        public virtual AttractionType? AttractionType { get; set; }
        public virtual ICollection<AttractionImage>? AttractionImages { get; set; }
    }
}
