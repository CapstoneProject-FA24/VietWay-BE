using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class TourTemplate
    {
        public int TourTemplateId { get; set; }
        public required string TourName { get; set; }
        public required string Description { get; set; }
        public required string Duration { get; set; }
        [ForeignKey(nameof(TourCategory))]
        public required int TourCategoryId { get; set; }
        public required string Policy { get; set; }
        public required string Note { get; set; }
        public required string Schedule { get; set; }
        public Enum.TourTemplateStatus Status { get; set; }
        public required DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(StaffInfo))]
        public int StaffId { get; set; }
        public required int CreatedBy { get; set; }

        public virtual StaffInfo? StaffInfo { get; set; }
        public virtual TourCategory? TourCategory { get; set; }
        public virtual ICollection<Tour>? Tours { get; set; }
        public virtual ICollection<TourTemplateProvince>? TourTemplateProvinces { get; set; }
        public virtual ICollection<TourTemplateAttraction>? TourTemplateAttractions { get; set; }
        public virtual ICollection<TourTemplateImage>? TourTemplateImages { get;set; }
    }
}
