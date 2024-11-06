using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Customer : SoftDeleteEntity
    {
        [Key]
        [ForeignKey(nameof(Account))]
        [StringLength(20)]
        public required string CustomerId { get; set; }
        [StringLength(100)]
        public required string FullName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        [ForeignKey(nameof(Province))]
        [StringLength(20)]
        public required string ProvinceId { get; set; }
        public required Gender Gender { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Province? Province { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<AttractionLike> AttractionLikes { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<AttractionReviewLike> AttractionReviewLikes { get;set; }
    }
}
