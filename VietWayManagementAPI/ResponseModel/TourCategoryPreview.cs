using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.ResponseModel
{
    public class TourCategoryPreview
    {
        public required string TourCategoryId { get; set; }
        public required string Name { get; set; }
    }
}
