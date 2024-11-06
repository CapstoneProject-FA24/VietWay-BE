using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    public class AttractionReviewLike
    {
        [StringLength(20)]
        public required string ReviewId { get; set; }
        [StringLength(20)]
        public required string CustomerId { get; set; }

        public virtual AttractionReview? AttractionReview { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
