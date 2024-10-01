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
    public class TourTemplate : CreatedByEntity<Staff>
    {
        [Key]
        [StringLength(20)]
        public required string TourTemplateId { get; set; }
        [StringLength(20)]
        public required string Code { get; set; }
        [StringLength(500)]
        public required string TourName { get; set; }
        public required string Description { get; set; }
        [ForeignKey(nameof(TourDuration))]
        public required string Duration { get; set; }
        [ForeignKey(nameof(TourCategory))]
        public required string TourCategoryId { get; set; }
        public required string Policy { get; set; }
        public required string Note { get; set; }
        public TourTemplateStatus Status { get; set; }

        public virtual TourDuration? TourDuration { get; set; }
        public virtual TourCategory? TourCategory { get; set; }
        public virtual ICollection<Tour>? Tours { get; set; }
        public virtual ICollection<TourTemplateProvince>? TourTemplateProvinces { get; set; }
        public virtual ICollection<TourTemplateSchedule>? TourTemplateSchedules { get; set; }
        public virtual ICollection<TourTemplateImage>? TourTemplateImages { get; set; }
    }
}
