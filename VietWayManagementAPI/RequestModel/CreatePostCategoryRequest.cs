using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class CreatePostCategoryRequest
    {
        public required string? Name { get; set; }
        public string? Description { get; set; }
    }
}
