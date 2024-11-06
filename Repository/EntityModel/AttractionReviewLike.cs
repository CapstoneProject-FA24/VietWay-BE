using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    [PrimaryKey(nameof(ReviewId), nameof(CustomerId))]
    public class AttractionReviewLike
    {
        [StringLength(20)]
        [ForeignKey(nameof(AttractionReview))]
        public required string ReviewId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }

        public virtual AttractionReview? AttractionReview { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
