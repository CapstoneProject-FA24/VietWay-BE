using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.DataTransferObject
{
    public class AttractionCategoryPreviewDTO
    {
        public string AttractionCategoryId { get; set; }
        public string Name { get; set; }
    }
}