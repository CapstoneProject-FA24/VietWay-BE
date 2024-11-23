using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.ResponseModel
{
    public class TourTemplatePreview
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Description { get; set; }
        public required string Duration { get; set; }
        public required string TourCategory { get; set; }
        public TourTemplateStatus Status { get; set; }
        public required decimal MinPrice { get; set; }
        public required decimal MaxPrice { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string CreatorName { get; set; }
        public required List<string> Provinces { get; set; }
        public required string ImageUrl { get; set; }
    }
}
