using Microsoft.EntityFrameworkCore;
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
    [Index(nameof(AttractionId), nameof(CustomerId), IsUnique = true)]
    public class AttractionReview : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string ReviewId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Attraction))]
        public required string AttractionId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }
        public required int Rating { get; set; }
        public string? Review { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<AttractionReviewLike>? AttractionReviewLikes { get; set; }
    }
}
