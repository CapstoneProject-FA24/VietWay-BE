using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourCategoryRequest
    {
        public required string? Name { get; set; }
        public required string? Description { get; set; }
    }
}
