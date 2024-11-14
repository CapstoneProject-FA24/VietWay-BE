using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Tour : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? TourId { get; set; }
        [ForeignKey(nameof(TourTemplate))]
        [StringLength(20)]
        [Required]
        public string? TourTemplateId { get; set; }
        [StringLength(255)]
        public string? StartLocation { get; set; }
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DefaultTouristPrice { get; set; }
        public DateTime? RegisterOpenDate { get; set; }
        public DateTime? RegisterCloseDate { get; set; }
        public int? MaxParticipant { get; set; }
        public int? MinParticipant { get; set; }
        [Required]
        public int CurrentParticipant { get; set; }
        [Required]
        public TourStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<Booking>? TourBookings { get; set; }
        public virtual ICollection<TourPrice>? TourPrices { get; set; }
        public virtual ICollection<TourRefundPolicy>? TourRefundPolicies { get; set; }

    }
}
