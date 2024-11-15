using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    public class AttractionReviewLike
    {
        [StringLength(20)]
        [Required]
        public string? ReviewId { get; set; }
        [StringLength(20)]
        [Required]
        public string? CustomerId { get; set; }

        public virtual AttractionReview? AttractionReview { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
