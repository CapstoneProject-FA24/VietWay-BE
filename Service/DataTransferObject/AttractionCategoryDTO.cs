using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.DataTransferObject
{
    public class AttractionCategoryDTO
    {
        public string AttractionCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}