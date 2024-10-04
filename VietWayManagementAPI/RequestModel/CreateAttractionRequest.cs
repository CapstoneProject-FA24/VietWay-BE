using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.RequestModel
{
    public class CreateAttractionRequest
    {
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
        [StringLength(500)]
        public string? ContactInfo { get; set; }
        [StringLength(2048)]
        public string? Website { get; set; }
        public string? Description { get; set; }
        [StringLength(20)]
        public required string ProvinceId { get; set; }
        [StringLength(20)]
        public required string AttractionTypeId { get; set; }
        [StringLength(50)]
        public string? GooglePlaceId { get; set; }

        public required List<IFormFile> Images { get; set; }
    }
}
