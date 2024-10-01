using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourTemplateRequest
    {
        [Required]
        public required string Code { get; set; }
        [Required]
        public required string TourName { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Duration { get; set; }
        [Required]
        public required long TourCategoryId { get; set; }
        [Required]
        public required string Policy { get; set; }
        [Required]
        public required string Note { get; set; }
        [Required]
        public required List<long> ProvinceId { get; set; }
        [Required]
        public required List<TemplateSchedule> Schedules { get; set; }
        [Required]
        public required List<IFormFile> Images { get; set; }
    }
    public class TemplateSchedule
    {
        [Required]
        public int DayNumber { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required List<long> AttractionId { get; set; }
    }
}
